
namespace Project.Scripts.Time.TimeScale
{
    public interface ITimeScaleManager
    {
        float CurrentTimeScale { get; }
        
        void SetTimeScale(float timeScale);
        void ResumeTimeScalePersisting(bool always = false);
        void PauseTimeScalePersisting(bool always = false);
    }
}