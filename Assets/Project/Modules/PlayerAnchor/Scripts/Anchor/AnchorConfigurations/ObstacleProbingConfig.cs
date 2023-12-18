using Project.Scripts.ProjectHelpers;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor.AnchorConfigurations
{
    [CreateAssetMenu(fileName = "ObstacleProbingConfig", 
        menuName = ScriptableObjectsHelper.PLAYER_ANCHOR_ASSETS_PATH + "ObstacleProbingConfig")]
    public class ObstacleProbingConfig : ScriptableObject
    {
        [Header("EDGE CASES")] 
        [SerializeField, Range(0.0f, 20.0f)] private float _heightToConsiderFloor = 10.0f;
        
        public float HeightToConsiderFloor => _heightToConsiderFloor;



        [Header("COLLISION DETECTION")] 
        [SerializeField] private LayerMask _obstaclesLayerMask;
        [SerializeField] private LayerMask _autoTargetLayerMask;
        [SerializeField, Range(0.0f, 90.0f)] private float _maxSteepAngleToConsiderFloor = 60.0f;
        private float _maxSteepDotToConsiderFloor;
        
        public LayerMask ObstaclesLayerMask => _obstaclesLayerMask;
        public LayerMask AutoTargetLayerMask => _autoTargetLayerMask;
        public float MaxSteepDotToConsiderFloor => _maxSteepDotToConsiderFloor;
        
        
        private void OnValidate()
        {
            _maxSteepDotToConsiderFloor = Mathf.Cos(_maxSteepAngleToConsiderFloor * Mathf.Deg2Rad);
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}