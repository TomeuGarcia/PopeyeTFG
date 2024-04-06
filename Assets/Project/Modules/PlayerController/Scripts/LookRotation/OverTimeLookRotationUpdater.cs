using UnityEngine;

namespace Popeye.Modules.PlayerController.LookRotation
{
    public class OverTimeLookRotationUpdater : ILookRotationUpdater
    {
        [System.Serializable]
        public class Configuration
        {
            [SerializeField, Range(0f, 2000f)] private float _rotationSpeed = 1000f;
            public float RotationSpeed => _rotationSpeed;
        }
        
        private readonly Transform _lookTransform;
        private readonly Configuration _configuration;

        public OverTimeLookRotationUpdater(Transform lookTransform, Configuration configuration)
        {
            _lookTransform = lookTransform;
            _configuration = configuration;
            
        }
        
        public void UpdateLocalRotation(Quaternion goalRotation)
        {
            _lookTransform.localRotation = Quaternion.RotateTowards(_lookTransform.localRotation, goalRotation,
                Time.deltaTime * _configuration.RotationSpeed);
        }
    }
}