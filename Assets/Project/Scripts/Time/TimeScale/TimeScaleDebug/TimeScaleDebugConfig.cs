using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Scripts.Time.TimeScale
{
    
    [CreateAssetMenu(fileName = "TimeScaleDebugConfig", 
        menuName = ScriptableObjectsHelper.TIME_ASSETS_PATH + "TimeScaleDebugConfig")]
    public class TimeScaleDebugConfig : ScriptableObject
    {
        [SerializeField] private bool _ignoreOnBuild = true;
        
        [SerializeField, Range(0.001f, 1.0f)] private float _slowTimeScale = 0.1f;
        [SerializeField, Range(1.0f, 4.0f)] private float _fastTimeScale = 2.0f;

        [SerializeField] private KeyCode _normalTimeKeyCode = KeyCode.N;
        [SerializeField] private KeyCode _slowTimeKeyCode = KeyCode.S;
        [SerializeField] private KeyCode _fastTimeKeyCode = KeyCode.F;
        
        
        public bool IgnoreOnBuild =>_ignoreOnBuild;
        
        public float NormalTimeScale => 1f;
        public float SlowTimeScale => _slowTimeScale;
        public float FastTimeScale => _fastTimeScale;

        public KeyCode NormalTimeKeyCode => _normalTimeKeyCode;
        public KeyCode SlowTimeKeyCode => _slowTimeKeyCode;
        public KeyCode FastTimeKeyCode => _fastTimeKeyCode;
    }
}