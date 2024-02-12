using System.Collections.Generic;
using AYellowpaper;
using NaughtyAttributes;
using UnityEngine;

namespace Popeye.Modules.WorldElements.MovableBlocks.GridMovement
{
    public class GridMovementArea : MonoBehaviour, IGridMovementArea
    {
        [SerializeField] private GridMovementAreaViewConfig _viewConfig;
        
        [Space(20)]
        [SerializeField] private InterfaceReference<IGridMovementActor, MonoBehaviour>[] _gridMovementActors;
        
        [Space(20)]
        [SerializeField] private bool _showAreas = true;
        [SerializeField] private List<RectangularAreaWrapper> _areaWrappers;

        
        private static Vector2 BOUNDS_OFFSET = Vector2.one * 0.05f;
        
        
        private void OnValidate()
        {
            for (int i = 0; i < _areaWrappers.Count; ++i)
            {
                _areaWrappers[i].OnValidateUpdateState();
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_showAreas)
            {
                return;
            }
            
            for (int i = 0; i < _areaWrappers.Count; ++i)
            {
                _areaWrappers[i].DrawGizmos();
            }
        }
#endif
        
        private void Awake()
        {
            foreach (var gridMovementActorReference in _gridMovementActors)
            {
                SetupGridMovementActor(gridMovementActorReference.Value);
            }

            OnValidate();
            
            for (int i = 0; i < _areaWrappers.Count; ++i)
            {
                RectangularArea rectangularArea = _areaWrappers[i].RectangularArea;
                GridMovementAreaViewHelper.CreateRectangularAreaView(_viewConfig, transform, rectangularArea, 
                    Vector3.up * 0.01f);
            }
        }

        private void SetupGridMovementActor(IGridMovementActor gridMovementActor)
        {
            gridMovementActor.Configure(this);
        }


        [Button("Spawn New Area")]
        private void SpawnNewArea()
        {
            RectangularArea rectangularArea = new RectangularArea(transform);
            RectangularAreaWrapper rectangularAreaWrapper = new RectangularAreaWrapper(rectangularArea);
            
            _areaWrappers.Add(rectangularAreaWrapper);
        }

        
        [Button("Reset Movement Actors With Children")]
        private void ResetMovementActorsWithChildren()
        {
            List<InterfaceReference<IGridMovementActor, MonoBehaviour>> _references = 
                new List<InterfaceReference<IGridMovementActor, MonoBehaviour>>(transform.childCount);

            for (int i = 0; i < transform.childCount; ++i)
            {
                if (transform.GetChild(i).TryGetComponent<IGridMovementActor>(out IGridMovementActor gridMovementActor))
                {
                    _references.Add(new InterfaceReference<IGridMovementActor, MonoBehaviour>(gridMovementActor));
                }
            }

            _gridMovementActors = _references.ToArray();
        }
        
        
        public bool CanMoveAfterDisplacement(IGridMovementActor gridMovementActor, Vector2 movementDisplacement)
        {
            Rect actorBounds = gridMovementActor.AreaBounds;
            actorBounds.center += movementDisplacement;
            
            for (int i = 0; i < _areaWrappers.Count; ++i)
            {
                RectangularArea rectangularArea = _areaWrappers[i].RectangularArea;

                Vector2 firstCorner = actorBounds.min + BOUNDS_OFFSET;
                Vector2 secondCorner = actorBounds.max - BOUNDS_OFFSET;

                if (rectangularArea.AreaContainsPoint(firstCorner))
                {
                    for (int j = 0; j < _areaWrappers.Count; ++j)
                    {
                        RectangularArea secondRectangularArea = _areaWrappers[j].RectangularArea;

                        if (secondRectangularArea.AreaContainsPoint(secondCorner))
                        {
                            return !OverlapsWithOtherActors(gridMovementActor, firstCorner, secondCorner);
                        }
                    }
                }
            }

            return false;
        }

        private bool OverlapsWithOtherActors(IGridMovementActor gridMovementActor, Vector2 firstCorner, Vector2 secondCorner)
        {
            for (int i = 0; i < _gridMovementActors.Length; ++i)
            {
                IGridMovementActor other = _gridMovementActors[i].Value;
                if (gridMovementActor == other)
                {
                    continue;
                }

                if (other.RectangularArea.AreaContainsPoint(firstCorner) ||
                    other.RectangularArea.AreaContainsPoint(secondCorner))
                {
                    return true;
                }
            }
            
            return false;
        }
        
    }
}