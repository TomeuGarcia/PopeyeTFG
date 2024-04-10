

using UnityEngine;

namespace Project.Scripts.Time.TimeScale
{
    public class UnityTimeScaleManager : ITimeScaleManager
    {
        public float CurrentTimeScale { get; private set; } = 1f;
        private float _persistingTimeScale = 1f;
        private int _pauseTimeScalePersistingCounter;
        
        public void SetTimeScale(float timeScale)
        {
            CurrentTimeScale = timeScale;
            UnityEngine.Time.timeScale = timeScale * _persistingTimeScale;
            _pauseTimeScalePersistingCounter = 0;
        }

        public void ResumeTimeScalePersisting(bool always = false)
        {
            _pauseTimeScalePersistingCounter = Mathf.Max(0,_pauseTimeScalePersistingCounter - 1);
            
            if (always)
            {
                _pauseTimeScalePersistingCounter = 0;
            }
            else if (_pauseTimeScalePersistingCounter != 0)
            {                
                return;
            }
            
            _persistingTimeScale = 1f;
            RefreshTimeScale();
        }

        public void PauseTimeScalePersisting(bool always = false)
        {
            ++_pauseTimeScalePersistingCounter;

            if (always)
            {
                _pauseTimeScalePersistingCounter = 0;
            }
            else if (_pauseTimeScalePersistingCounter != 1)
            {
                return;
            }
        
            _persistingTimeScale = 0f;
            RefreshTimeScale();
        }

        private void RefreshTimeScale()
        {
            SetTimeScale(CurrentTimeScale);
        }
    }
}