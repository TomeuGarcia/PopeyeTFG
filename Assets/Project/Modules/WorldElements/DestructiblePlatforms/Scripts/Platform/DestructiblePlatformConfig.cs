using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Modules.WorldElements.DestructiblePlatforms
{
    [CreateAssetMenu(fileName = "DestructiblePlatformConfig", 
        menuName = ScriptableObjectsHelper.WORLDELEMENTS_ASSETS_PATH + "DestructiblePlatformConfig")]
    public class DestructiblePlatformConfig : ScriptableObject
    {
        [SerializeField, Range(0.01f, 0.5f)] private float _breakOverTimeStartDelay = 0.25f;
        [SerializeField, Range(0.01f, 20.0f)] private float _breakOverTimeDuration = 2.0f;
        [SerializeField, Range(0.01f, 0.5f)] private float _enterBrokenStateDelay = 0.15f;
        [SerializeField, Range(0.01f, 20.0f)] private float _brokenStateStateDuration = 3.0f;

        
        public float BreakOverTimeStartDelay => _breakOverTimeStartDelay;
        public float BreakOverTimeDuration => _breakOverTimeDuration;
        public float EnterBrokenStateDelay => _enterBrokenStateDelay;
        public float BrokenStateDuration => _brokenStateStateDuration;
    }
}