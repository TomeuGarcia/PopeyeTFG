using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.WorldElements.AnchorTriggerables
{
    [CreateAssetMenu(fileName = "TimerPressurePlatePulseConfig", 
        menuName = ScriptableObjectsHelper.WORLDELEMENTS_ASSETS_PATH + "TimerPressurePlatePulseConfig")]
    public class TimerPressurePlatePulseConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 5.0f)] private float _normalPulseDuration = 0.75f;
        [SerializeField, Range(0.0f, 5.0f)] private float _fastPulseDuration = 0.25f;
        
        public float NormalPulseDuration => _normalPulseDuration;
        public float FastPulseDuration => _fastPulseDuration;
    }
}