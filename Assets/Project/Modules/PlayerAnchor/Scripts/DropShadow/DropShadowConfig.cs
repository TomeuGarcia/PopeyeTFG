using System;
using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using Popeye.ProjectHelpers;
using Popeye.Scripts.Collisions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.DropShadow
{
    
    [CreateAssetMenu(fileName = "DropShadowConfig", 
        menuName = ScriptableObjectsHelper.PLAYER_ANCHOR_ASSETS_PATH + "DropShadowConfig")]
    public class DropShadowConfig : ScriptableObject
    {
        [SerializeField] private CollisionProbingConfig _floorCOllisionProbingConfig;
        [SerializeField] private Vector3 _probeOffset = new Vector3(0,1.5f, 0);
        public LayerMask ObstacleLayerMask => _floorCOllisionProbingConfig.CollisionLayerMask;
        public float ProbeDistance => _floorCOllisionProbingConfig.ProbeDistance;
        public Vector3 ProbeOffset => _probeOffset;


        [SerializeField, Range(0, 0.1f)] private float _displacementFromFloor = 0.01f;
        public float DisplacementFromFloor => _displacementFromFloor;
        
        
        [SerializeField, Range(0, 100)] private float _closestDistance = 2.0f;
        [SerializeField, Range(0, 100)] private float _furthestDistance = 10.0f;
        
        [SerializeField, Range(0,5)] private float _sizeClosestDistance = 1.0f;
        [SerializeField, Range(0,5)] private float _sizeFurthestDistance = 0.2f;
        
        public float FurthestDistance => _furthestDistance;
        
        
        [SerializeField, Range(0,1)] private float _showDuration = 0.1f;
        [SerializeField, Range(0,1)] private float _hideDuration = 0.2f;
        
        public float ShowDuration => _showDuration;
        public float HideDuration => _hideDuration;
        

        private void OnValidate()
        {
            if (_closestDistance > _furthestDistance)
            {
                _closestDistance = _furthestDistance;
            }
            
            if (_sizeClosestDistance < _sizeFurthestDistance)
            {
                _sizeClosestDistance = _sizeFurthestDistance;
            }
        }

        private void Awake()
        {
            OnValidate();
        }


        public float GetSizeFromDistance(float distanceFromFloor)
        {
            float distanceT = Mathf.Max(0, distanceFromFloor - _closestDistance) / (_furthestDistance - _closestDistance);
            distanceT = Mathf.Min(1, distanceT);

            return Mathf.Lerp(_sizeClosestDistance, _sizeFurthestDistance, distanceT);
        }
    }
}