using NaughtyAttributes;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Camera.CameraShake
{
    [CreateAssetMenu(fileName = "CameraShakeConfig", 
        menuName = ScriptableObjectsHelper.CAMERA_ASSETS_PATH + "CameraShakeConfig")]
    public class CameraShakeConfig : ScriptableObject
    {
        public enum ShakeDirectionType
        {
            Random,
            Deterministic
        }
        
        
        [SerializeField, Range(0.0f, 10.0f)] private float _strength = 1.0f;
        [SerializeField, Range(0.0f, 10.0f)] private float _duration = 0.5f;

        [Space(20)]
        [SerializeField] private ShakeDirectionType _directionType = ShakeDirectionType.Deterministic;
        
        [ShowIf("IsDeterministicDirection")]
        [SerializeField] private Vector3 _direction = Vector3.down;
        
        [Space(20)]
        [SerializeField] private AnimationCurve _easeCurve = AnimationCurve.Linear(0,0,1,1);
        
        
        
        public float Strength => _strength;
        public float Duration => _duration;
        public ShakeDirectionType DirectionType => _directionType;
        public Vector3 Direction => IsDeterministicDirection() ? _direction : Random.insideUnitSphere.normalized;
        public AnimationCurve EaseCurve => _easeCurve;


        private bool IsDeterministicDirection()
        {
            return _directionType == ShakeDirectionType.Deterministic;
        }
    }
}