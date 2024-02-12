using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.WorldElements.MovableBlocks.GridMovement
{
    [System.Serializable]
    public class RectangularArea
    {
        [SerializeField] private Transform _centerTransform;
        [SerializeField] private Vector2Int _centerOffset = Vector2Int.zero;
        [SerializeField] private Vector2Int _size = Vector2Int.one * 2;

        private Vector2 _halfSize;
        
        public Vector3 CenterOffset3D { get; private set; }
        public Vector3 OffsetCorner_RightForward { get; private set; }
        public Vector3 OffsetCorner_RightBack { get; private set; }
        public Vector3 OffsetCorner_LeftBack { get; private set; }
        public Vector3 OffsetCorner_LeftForward { get; private set; }

        public Vector3 Center => _centerTransform.position + CenterOffset3D;
        public Vector3 Corner_RightForward => Center + OffsetCorner_RightForward;
        public Vector3 Corner_RightBack => Center + OffsetCorner_RightBack;
        public Vector3 Corner_LeftBack => Center + OffsetCorner_LeftBack;
        public Vector3 Corner_LeftForward => Center + OffsetCorner_LeftForward;

        public Rect AreaBounds { get; private set; }
        
        
        public RectangularArea(Transform centerTransform)
        {
            _centerTransform = centerTransform;
            _centerOffset = Vector2Int.zero;
            _size = Vector2Int.one * 2;
            
            UpdateState();
        }


        public void UpdateState()
        {
            _halfSize =  new Vector2(_size.x, _size.y) / 2;

            OffsetCorner_RightForward = (Vector3.right * _halfSize.x) + (Vector3.forward * _halfSize.y);
            OffsetCorner_RightBack    = (Vector3.right * _halfSize.x) + (Vector3.back * _halfSize.y);
            OffsetCorner_LeftBack     = (Vector3.left * _halfSize.x) + (Vector3.back * _halfSize.y);
            OffsetCorner_LeftForward  = (Vector3.left * _halfSize.x) + (Vector3.forward * _halfSize.y);

            CenterOffset3D = new Vector3(_centerOffset.x, 0, _centerOffset.y);

            Vector2 areaPosition = new Vector2(Center.x, Center.z) - new Vector2(OffsetCorner_RightForward.x, OffsetCorner_RightForward.z);

            AreaBounds = new Rect(areaPosition, _size);
        }
        
        public bool AreaContainsPoint(Vector2 point)
        {
            return AreaBounds.Contains(point);
        }
    }
    
}