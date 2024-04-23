using Project.Scripts.Core.Transitions;

namespace Project.Scripts.Time.TimeHitStop
{
    public class HitStop
    {
        private readonly HitStopConfig _config;
        
        public float RealtimeDuration => _config.RealtimeDuration;
        public float TimeScale => _config.TimeScale;
        public HitStopTransitionType TransitionType => _config.TransitionType;
        public TransitionConfig TransitionConfig => _config.TransitionConfig;
        

        public HitStop(HitStopConfig config)
        {
            _config = config;
        }
    }
}