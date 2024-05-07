using System.Collections;
using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.WorldElements.AnchorTriggerables
{
    public class TimerButtonAudioPlayer : MonoBehaviour
    {
        [SerializeField] private TimerPressurePlateAudios _config;
        [SerializeField] private TimerPressurePlatePulseConfig _pulseConfig;

        private Coroutine _tickDownSounds;
        
        
        public void PlayActivatedSound()
        {
            _config.PlayActivatedSound(gameObject);
        }
        
        public void StartPlayingTickDown(float totalDuration, float durationBeforeFinish)
        {
            _tickDownSounds = StartCoroutine(DoPlayTickDown(totalDuration, durationBeforeFinish));
        }
        
        public void StopPlayingTickDown()
        {
            if (_tickDownSounds != null)
            {
                StopCoroutine(_tickDownSounds);
            }
        }

        private IEnumerator DoPlayTickDown(float totalDuration, float durationBeforeFinish)
        {
            float slowCountdownDuration = totalDuration - durationBeforeFinish;
        
            Timer soundsTimer = new Timer(slowCountdownDuration);
            while (!soundsTimer.HasFinished())
            {
                float waitDuration = Mathf.Min(soundsTimer.RemainingTime, _pulseConfig.NormalPulseDuration);
                soundsTimer.Update(waitDuration);

                _config.PlayTickSound(gameObject);
                
                yield return new WaitForSeconds(waitDuration);
            }
            
            
            soundsTimer = new Timer(durationBeforeFinish);
            while (!soundsTimer.HasFinished())
            {
                float waitDuration = Mathf.Min(soundsTimer.RemainingTime, _pulseConfig.FastPulseDuration);
                soundsTimer.Update(waitDuration);

                _config.PlayFastTickSound(gameObject);
                
                yield return new WaitForSeconds(waitDuration);
            }
            
            _tickDownSounds = null;
        }
        
    }
}