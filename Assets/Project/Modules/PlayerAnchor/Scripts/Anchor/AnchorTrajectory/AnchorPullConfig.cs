using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Modules.PlayerAnchor.Anchor
{
    
    [CreateAssetMenu(fileName = "AnchorPullConfig", 
        menuName = AnchorConfigHelper.SO_ASSETS_PATH + "AnchorPullConfig")]
    public class AnchorPullConfig : ScriptableObject
    {
        
        [Header("DISTANCES")]
        [SerializeField, Range(0.01f, 20.0f)] private float _minPullDistance = 2.0f;
        [SerializeField, Range(0.01f, 20.0f)] private float _maxPullDistance = 10.0f;
        
        public float MinPullDistance => _minPullDistance;
        public float MaxPullDistance => _maxPullDistance;
        
        
        [Header("DURATIONS")]
        [SerializeField, Range(0.01f, 10.0f)] private float _minPullMoveDuration = 0.2f;
        [SerializeField, Range(0.01f, 10.0f)] private float _maxPullMoveDuration = 0.5f;
        
        public float MinPullMoveDuration => _minPullMoveDuration;
        public float MaxPullMoveDuration => _maxPullMoveDuration;
        
        
        
        [Header("PULL TRAJECTORY BEND")] 
        [SerializeField, Range(2, 16)] private int _trajectoryBendSharpness = 5;

        public int TrajectoryBendSharpness => _trajectoryBendSharpness;


        private void OnValidate()
        {
            _maxPullDistance = Mathf.Max(_maxPullDistance, _minPullDistance);
        }
    }
}