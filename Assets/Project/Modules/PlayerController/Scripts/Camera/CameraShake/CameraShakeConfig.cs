using Project.Scripts.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Camera.CameraShake
{
    [CreateAssetMenu(fileName = "CameraShakeConfig", 
        menuName = ScriptableObjectsHelper.CAMERA_ASSETS_PATH + "CameraShakeConfig")]
    public class CameraShakeConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 10.0f)] private float _strength = 1.0f;
        [SerializeField, Range(0.0f, 10.0f)] private float _duration = 0.5f;
        [SerializeField] private AnimationCurve _easeCurve = AnimationCurve.Linear(0,0,1,1);
        
        public float Strength => _strength;
        public float Duration => _duration;
        public AnimationCurve EaseCurve => _easeCurve;
    }
}