using NaughtyAttributes;
using Popeye.ProjectHelpers;
using Project.Scripts.Core.Transitions;
using UnityEngine;

namespace Project.Scripts.Time.TimeHitStop
{
    
    [CreateAssetMenu(fileName = "HitStopConfig_NAME", 
        menuName = ScriptableObjectsHelper.HITSTOP_ASSETS_PATH + "HitStopConfig")]
    public class HitStopConfig : ScriptableObject
    {
        [SerializeField] private HitStopTransitionType _transitionType = HitStopTransitionType.Instant;
        [SerializeField, Range(0.001f, 1.0f)] private float _realtimeDuration = 0.2f;
        [SerializeField, Range(0.0f, 1.0f)] private float _timeScale = 0.1f;

        [Space(40)]
        [ShowIf("TransitionTypeIsNotInstant"), Expandable]
        [SerializeField] private TransitionConfig _transitionConfig;


        public HitStopTransitionType TransitionType => _transitionType;
        public float RealtimeDuration => _realtimeDuration;
        public float TimeScale => _timeScale;
        public TransitionConfig TransitionConfig => _transitionConfig;


        private bool TransitionTypeIsNotInstant()
        {
            return _transitionType != HitStopTransitionType.Instant;
        }

        
        private void OnValidate()
        {
            if (TransitionTypeIsNotInstant() && _transitionConfig != null)
            {
                _transitionConfig.SetMidpointDuration(_realtimeDuration);
            }
        }

        private void Awake()
        {
            OnValidate();
        }
    }
    
}