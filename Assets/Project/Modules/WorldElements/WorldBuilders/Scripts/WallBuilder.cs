using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.WorldElements.WorldBuilders
{
    public class WallBuilder : MonoBehaviour
    {
        [System.Serializable]
        public class Block
        {
            [SerializeField] private Vector2 _size = Vector2.one;
            private Vector2 _halfSize;
            public Vector2 Size => _size;
            public float Length => Size.y;

            public Vector3[] ToFrame(Vector3 center, Quaternion rotation)
            {
                Vector3[] framePositions = new Vector3[4];

                framePositions[0] = center + (rotation * (Vector3.right * _halfSize.x +   Vector3.forward * _halfSize.y));
                framePositions[1] = center + (rotation * (Vector3.right * _halfSize.x +   Vector3.back * _halfSize.y));
                framePositions[2] = center + (rotation * (Vector3.left * _halfSize.x +    Vector3.back * _halfSize.y));
                framePositions[3] = center + (rotation * (Vector3.left * _halfSize.x +    Vector3.forward * _halfSize.y));

                return framePositions;
            }

            public void UpdateHalfSize()
            {
                _halfSize = Size / 2;
            }
        }

        [Header("PARENTS")] 
        [SerializeField] private Transform _cornerWallsParent;
        [SerializeField] private Transform _fillWallsParent;
        [SerializeField] private GameObject _collidersParent;
        
        [Header("CONFIG")]
        [Expandable] [SerializeField] private WallBuilderConfig _config;
        
        [Header("POINTS")]
        [SerializeField] private Vector3[] _points = { 
            Vector3.left * 2, 
            Vector3.right * 2, 
            Vector3.right * 2 + Vector3.forward * 3 
        };

        public Vector3[] Points => _points;
        public Block CornerBlock => _config.CornerBlock;
        public Block FillBlock => _config.FillBlock;
        public WallBuilderConfig.EditorViewConfig EditorView => _config.EditorView;

        [HideInInspector] public Vector2 moveBy = Vector2.zero;
        [HideInInspector] public Vector2Int selectedPointsRange = Vector2Int.zero;


        private void OnValidate()
        {
            if (IsReadyForEditor())
            {
                CornerBlock.UpdateHalfSize();
                FillBlock.UpdateHalfSize();
            }
        }

        private void Awake()
        {
            OnValidate();

            if (!_cornerWallsParent) _cornerWallsParent = transform;
            if (!_fillWallsParent) _fillWallsParent = transform;
            if (!_collidersParent) _collidersParent = gameObject;
        }

        private void Start()
        {
            if (_points.Length > 0)
            {
                FillWalls();
                CreateFakeMesh();
            }
        }

        public void AddPoint()
        {
            Array.Resize(ref _points, _points.Length + 1);

            if (_points.Length > 1)
            {
                _points[^1] = _points[^2] + Vector3.right;    
            }
            else
            {
                _points[0] = Vector3.right;
            }
        }

        public bool IsReadyForEditor()
        {
            return _config != null && _points.Length > 1;
        }


        private void FillWalls()
        {
            for (int i = 0; i < _points.Length; ++i)
            {
                Vector3 point = transform.TransformPoint(_points[i]);
                CreateCornerWall(point);
            }
            
            if (FillBlock.Length < 0.01f)
            {
                return;
            }
            
            for (int i = 1; i < _points.Length; ++i)
            {
                Vector3 previousPoint = transform.TransformPoint(_points[i - 1]);
                Vector3 currentPoint = transform.TransformPoint(_points[i]);
                

                Vector3 previousToCurrent = currentPoint - previousPoint;
                float previousToCurrentDistance = previousToCurrent.magnitude;
                Vector3 previousToCurrentDirection = previousToCurrent / previousToCurrentDistance;
                
                Quaternion offsetRotation = Quaternion.LookRotation(previousToCurrentDirection, Vector3.up);
                
                float fillLength = previousToCurrentDistance - CornerBlock.Length;

                
                CreateFillWalls(previousPoint, previousToCurrentDirection, previousToCurrentDistance, offsetRotation,
                    fillLength);
                
                CreateColliderPreviousToCurrent(i, previousToCurrentDirection, previousToCurrentDistance, offsetRotation,
                    fillLength);
                    
            }

            CreateColliderForLast();
        }


        private void CreateCornerWall(Vector3 position)
        {
            Instantiate(_config.CornerBlockPrefab, position, Quaternion.identity, _cornerWallsParent);
        }

        private void CreateFillWalls(Vector3 previousPoint, Vector3 previousToCurrentDirection, float previousToCurrentDistance,
            Quaternion rotation, float fillLength)
        {
            float distanceCounter = CornerBlock.Length / 2 + FillBlock.Length / 2;
            
            for (; distanceCounter < fillLength; distanceCounter += FillBlock.Length)
            {
                Vector3 fillPosition = previousPoint + (previousToCurrentDirection * distanceCounter);
                Instantiate(_config.FillBlockPrefab, fillPosition, rotation, _fillWallsParent);
            }
        }
        

        private void CreateColliderPreviousToCurrent(int index, Vector3 previousToCurrentDirection, float previousToCurrentDistance,  
            Quaternion rotation, float fillLength)
        {
            Vector3 colliderPosition = _points[index - 1] + (previousToCurrentDirection * (fillLength / 2));
            Vector3 colliderSize = rotation * 
                                   (new Vector3(_config.ColliderWidth, _config.ColliderHeight, previousToCurrentDistance)); 
                
            DoCreateCollider(colliderPosition, colliderSize);
        }
        
        private void CreateColliderForLast()
        {
            Vector3 colliderPosition = _points[^1];
            Vector3 colliderSize = new Vector3(_config.ColliderWidth, _config.ColliderHeight, CornerBlock.Length);

            DoCreateCollider(colliderPosition, colliderSize);
        }

        private void DoCreateCollider(Vector3 colliderPosition, Vector3 colliderSize)
        {
            colliderSize.x = Mathf.Abs(colliderSize.x);
            colliderSize.y = Mathf.Abs(colliderSize.y);
            colliderSize.z = Mathf.Abs(colliderSize.z);
            
            BoxCollider boxCollider = _collidersParent.AddComponent<BoxCollider>();
            boxCollider.center = colliderPosition + (Vector3.up * _config.HalfColliderHeight);
            boxCollider.size = colliderSize;
        }



        private void CreateFakeMesh()
        {
            _collidersParent.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = _collidersParent.AddComponent<MeshRenderer>();
            meshRenderer.material = _config.FakeMeshMaterial;
            meshRenderer.enabled = false;
        }


        public void CenterAroundPivot()
        {
            Vector2 minimums = Vector2.one * float.MaxValue;
            Vector2 maximums = Vector2.one * float.MinValue;
            
            foreach (Vector3 point in _points)
            {
                if (point.x < minimums.x) minimums.x = point.x;
                if (point.z < minimums.y) minimums.y = point.z;
                
                if (point.x > maximums.x) maximums.x = point.x;
                if (point.z > maximums.y) maximums.y = point.z;
            }

            
            Vector2 offset = (minimums + maximums) / 2;

            MovePointsRangeByAmount(offset, 0, _points.Length);
        }

        public void MovePointsRangeByAmount(Vector2 moveAmount, int startInclusive, int endExclusive)
        {
            for (int i = startInclusive; i < endExclusive; ++i)
            {
                _points[i] -= new Vector3(moveAmount.x, 0, moveAmount.y);
            }
        }
        
    }
}