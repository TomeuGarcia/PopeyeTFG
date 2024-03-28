
namespace Project.Scripts.Time.TimeScale
{
    public class UnityTimeScaleManager : ITimeScaleManager
    {
        public float CurrentTimeScale { get; private set; } = 1f;
        private float _persistingTimeScale = 1f;
        
        
        public void SetTimeScale(float timeScale)
        {
            CurrentTimeScale = timeScale;
            UnityEngine.Time.timeScale = timeScale * _persistingTimeScale;
        }

        public void SetPersistingTimeScale(float persistingTimeScale)
        {
            _persistingTimeScale = persistingTimeScale;
            RefreshTimeScale();
        }

        private void RefreshTimeScale()
        {
            SetTimeScale(CurrentTimeScale);
        }
    }
}