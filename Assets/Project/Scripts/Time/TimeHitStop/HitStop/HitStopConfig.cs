using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Scripts.Time.TimeHitStop
{
    
    [CreateAssetMenu(fileName = "HitStopConfig_NAME", 
        menuName = ScriptableObjectsHelper.HITSTOP_ASSETS_PATH + "HitStopConfig")]
    public class HitStopConfig : ScriptableObject
    {
        [SerializeField, Range(0.001f, 1.0f)] private float _realtimeDuration = 0.2f;
        [SerializeField, Range(0.0f, 1.0f)] private float _timeScale = 0.1f;
        
        
        public float RealtimeDuration => _realtimeDuration;
        public float TimeScale => _timeScale;
    }
}