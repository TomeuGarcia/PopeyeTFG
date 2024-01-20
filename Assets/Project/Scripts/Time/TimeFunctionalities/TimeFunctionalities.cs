using Project.Scripts.Time.TimeHitStop;
using Project.Scripts.Time.TimeScale;

namespace Project.Scripts.Time.TimeFunctionalities
{
    public class TimeFunctionalities : ITimeFunctionalities
    {
        public ITimeScaleManager TimeScaleManager { get; private set; }
        public IHitStopManager HitStopManager { get; private set; }


        public TimeFunctionalities(ITimeScaleManager timeScaleManager, IHitStopManager hitStopManager)
        {
            TimeScaleManager = timeScaleManager;
            HitStopManager = hitStopManager;
        }
    }
}