using System;
using System.Collections;
using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.WorldElements.AnchorTriggerables
{
    public class TimerPressurePlateFillView : MonoBehaviour, ITimerPressurePlateView
    {
        [Header("COMPONENTS")]
        [SerializeField] private MeshRenderer _buttonMesh;
        private Material _timerMaterial;

        private Coroutine _countdownAnimation = null;


        
        private void Awake()
        {
            _timerMaterial = _buttonMesh.material;
            SetFillValue(0);
        }


        public void SetTimerStart()
        {
            SetFillValue(1);
        }

        public void StartTimerCountdown(float totalDuration, float durationBeforeFinish)
        {
            _countdownAnimation = StartCoroutine(DoStartTimerCountdown(totalDuration, durationBeforeFinish));
        }
        
        private IEnumerator DoStartTimerCountdown(float totalDuration, float durationBeforeFinish)
        {
            Timer pressedTimer = new Timer(totalDuration);
            while (!pressedTimer.HasFinished())
            {
                pressedTimer.Update(Time.deltaTime);
                SetFillValue(1-pressedTimer.GetCounterRatio01());
                
                yield return null;
            }

            _countdownAnimation = null;
        }

        public void CancelTimerCountdown()
        {
            if (_countdownAnimation != null)
            {
                StopCoroutine(_countdownAnimation);
            }        
        }

        public void CancelAndLockTimerCountdown()
        {
            CancelTimerCountdown();
            SetFillValue(1);
        }


        private void SetFillValue(float fillValue)
        {
            _timerMaterial.SetFloat("_FillT", fillValue);
        }
    }
}