using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Scripts.Time.TimeHitStop
{
    
    [CreateAssetMenu(fileName = "HitStopManagerConfig", 
        menuName = ScriptableObjectsHelper.HITSTOP_ASSETS_PATH + "HitStopManagerConfig")]
    public class HitStopManagerConfig : ScriptableObject
    {
        [SerializeField, Range(1, 20)] private int _maxSimultaneousHitStops = 5;
        [SerializeField, Range(0.001f, 0.5f)] private float _delayBetweenHitStops = 0.1f;
        
        
        public int MaxSimultaneousHitStops => _maxSimultaneousHitStops;
        public float DelayBetweenHitStops => _delayBetweenHitStops;
        public float DefaultTimeScale => 1f;
    }
}