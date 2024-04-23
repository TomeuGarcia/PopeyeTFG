using UnityEngine;

namespace Popeye.InverseKinematics.FABRIK
{
    [System.Serializable]
    public class FABRIKControllerConfig
    {
        [SerializeField, Range(0f, 5f)] private float _angleThreshold = 1.0f;
        [SerializeField, Range(0f, 1f)] private float _distanceToEndEffectorTolerance = 0.01f;
        [SerializeField, Range(1, 10)] private int _maxTries = 10;
        [SerializeField, Range(0f, 200f)] private float _lerpSpeed = 100f;
        
        
        public float AngleThreshold => _angleThreshold;
        public float DistanceToEndEffectorTolerance => _distanceToEndEffectorTolerance;
        public int MaxTries => _maxTries;
        public float LerpSpeed => _lerpSpeed;
    }
}