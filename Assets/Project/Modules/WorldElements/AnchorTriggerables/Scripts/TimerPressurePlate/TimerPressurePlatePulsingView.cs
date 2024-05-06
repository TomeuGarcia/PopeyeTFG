using System.Collections;
using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.WorldElements.AnchorTriggerables
{
    public class TimerPressurePlatePulsingView : MonoBehaviour, ITimerPressurePlateView
    {

        [SerializeField] private TimerPressurePlatePulseConfig _pulseConfig;
        [Header("COMPONENTS")]
        [SerializeField] private MeshRenderer _buttonMesh;
        private Material _pulsingMaterial;

        private Coroutine _countdownAnimation = null;
        private Coroutine _pulsingAnimation = null;


        
        private void Awake()
        {
            _pulsingMaterial = _buttonMesh.material;
            SetPulseOff();
        }
    
        public void SetTimerStart()
        {
            UpdatePulse(0);
        }

        public void StartTimerCountdown(float totalDuration, float durationBeforeFinish)
        {
            SetLockedState(false);
            _countdownAnimation = StartCoroutine(DoStartTimerCountdown(totalDuration, durationBeforeFinish));
        }
        
        private IEnumerator DoStartTimerCountdown(float totalDuration, float durationBeforeFinish)
        {
            UpdateIsPulsing(true);
        
            float slowCountdownDuration = totalDuration - durationBeforeFinish;
            
            Timer pressedTimer = new Timer(slowCountdownDuration);
            while (!pressedTimer.HasFinished())
            {
                float waitDuration = Mathf.Min(pressedTimer.RemainingTime, _pulseConfig.NormalPulseDuration);
                pressedTimer.Update(waitDuration);

                
                _pulsingAnimation = StartCoroutine(DoPulse(waitDuration));
                yield return _pulsingAnimation;
            }
            
            
            pressedTimer = new Timer(durationBeforeFinish);
            while (!pressedTimer.HasFinished())
            {
                float waitDuration = Mathf.Min(pressedTimer.RemainingTime, _pulseConfig.FastPulseDuration);
                pressedTimer.Update(waitDuration);

                
                _pulsingAnimation = StartCoroutine(DoPulse(waitDuration));
                yield return _pulsingAnimation;
            }

            SetPulseOff();
            _countdownAnimation = null;
            
            UpdateIsPulsing(false);
        }
        
        
        public void CancelTimerCountdown()
        {
            if (_countdownAnimation != null)
            {
                StopCoroutine(_countdownAnimation);
            }
            if (_pulsingAnimation != null)
            {
                StopCoroutine(_pulsingAnimation);
            }
        }

        public void CancelAndLockTimerCountdown()
        {
            CancelTimerCountdown();

            SetLockedState(true);
        }
        

        private IEnumerator DoPulse(float duration)
        {
            Timer pulseTimer = new Timer(duration);

            while (!pulseTimer.HasFinished())
            {
                pulseTimer.Update(Time.deltaTime);

                UpdatePulse(pulseTimer.GetCounterRatio01());
                
                yield return null;
            }
            
        }

        private void SetPulseOff()
        {
            UpdatePulse(0.5f);
            UpdateIsPulsing(false);
        }
        private void UpdatePulse(float t)
        {
            _pulsingMaterial.SetFloat("_PulsingT", t);
        }
        private void UpdateIsPulsing(bool isPulsing)
        {
            _pulsingMaterial.SetFloat("_IsPulsing", isPulsing ? 1 : 0);
        }
        private void SetLockedState(bool isLocked)
        {
            _pulsingMaterial.SetFloat("_PulsingIsAlwaysOn", isLocked ? 1 : 0);
        }
    }
}