namespace Project.Scripts.Time.TimeHitStop
{
    public class HitStop
    {
        private readonly HitStopConfig _config;
        
        public float RealtimeDuration => _config.RealtimeDuration;
        public float TimeScale => _config.TimeScale;
        

        public HitStop(HitStopConfig config)
        {
            _config = config;
        }
    }
}