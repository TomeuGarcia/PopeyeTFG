using Project.Scripts.Time.TimeHitStop;
using Project.Scripts.Time.TimeScale;

namespace Project.Scripts.Time.TimeFunctionalities
{
    public interface ITimeFunctionalities
    {
        ITimeScaleManager TimeScaleManager { get; }
        IHitStopManager HitStopManager { get; }
    }
}