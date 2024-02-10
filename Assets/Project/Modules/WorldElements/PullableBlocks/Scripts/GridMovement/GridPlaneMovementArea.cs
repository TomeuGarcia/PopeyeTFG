using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.WorldElements.PullableBlocks.GridMovement
{
    public class GridPlaneMovementArea : MonoBehaviour
    {
        [System.Serializable]
        public class AreaPlaneWrapper
        {
            public AreaPlaneWrapper(RectangularAreaPlane rectangularAreaPlane)
            {
                _rectangularAreaPlane = rectangularAreaPlane;
                _drawColor = Color.yellow;
            }
            
            private RectangularAreaPlane _rectangularAreaPlane;
            [SerializeField] private Color _drawColor;

            public RectangularAreaPlane RectangularAreaPlane => _rectangularAreaPlane;
            public Color DrawColor => _drawColor;
        }
        
        
        [SerializeField] private List<AreaPlaneWrapper> _areaPlaneWrappers = new List<AreaPlaneWrapper>();
        
        public List<AreaPlaneWrapper> AreaPlaneWrappers => _areaPlaneWrappers;
        
        
        public void SpawnAreaPlane()
        {
            RectangularAreaPlane rectangularAreaPlane = new RectangularAreaPlane(transform.position, 2f);
            _areaPlaneWrappers.Add(new AreaPlaneWrapper(rectangularAreaPlane));
        }
        
        public void ClearAreaPlanes()
        {
            _areaPlaneWrappers.Clear();
        }
        
    }
}