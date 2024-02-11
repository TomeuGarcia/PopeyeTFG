using UnityEngine;

namespace Popeye.Modules.WorldElements.PullableBlocks.GridMovement
{
    [System.Serializable]
    public class RectangularArea
    {
        [SerializeField] private Transform _centerTransform;
        [SerializeField] private Vector2Int _centerOffset = Vector2Int.zero;
        [SerializeField] private Vector2Int _size = Vector2Int.one * 2;

        private Vector2 _halfSize;
        
        private Vector3 _centerOffset3D;
        private Vector3 _offsetCorner_RightForward;
        private Vector3 _offsetCorner_RightBack;
        private Vector3 _offsetCorner_LeftBack;
        private Vector3 _offsetCorner_LeftForward;

        public Vector3 Center => _centerTransform.position + _centerOffset3D;
        public Vector3 Corner_RightForward => Center + _offsetCorner_RightForward;
        public Vector3 Corner_RightBack => Center + _offsetCorner_RightBack;
        public Vector3 Corner_LeftBack => Center + _offsetCorner_LeftBack;
        public Vector3 Corner_LeftForward => Center + _offsetCorner_LeftForward;

        public Rect AreaBounds { get; private set; }
        
        
        public RectangularArea(Transform centerTransform)
        {
            _centerTransform = centerTransform;
            UpdateState();
        }


        public void UpdateState()
        {
            _halfSize =  new Vector2(_size.x, _size.y) / 2;

            _offsetCorner_RightForward = (Vector3.right * _halfSize.x) + (Vector3.forward * _halfSize.y);
            _offsetCorner_RightBack    = (Vector3.right * _halfSize.x) + (Vector3.back * _halfSize.y);
            _offsetCorner_LeftBack     = (Vector3.left * _halfSize.x) + (Vector3.back * _halfSize.y);
            _offsetCorner_LeftForward  = (Vector3.left * _halfSize.x) + (Vector3.forward * _halfSize.y);

            _centerOffset3D = new Vector3(_centerOffset.x, 0, _centerOffset.y);

            Vector2 areaPosition = new Vector2(Center.x, Center.z) - new Vector2(_offsetCorner_RightForward.x, _offsetCorner_RightForward.z);

            AreaBounds = new Rect(areaPosition, _size);
        }
        
        public bool AreaContainsPoint(Vector2 point)
        {
            return AreaBounds.Contains(point);
        }
    }
    
}