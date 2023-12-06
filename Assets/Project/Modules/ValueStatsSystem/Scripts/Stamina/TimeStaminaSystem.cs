using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Timer = Popeye.Modules.Utilities.Timer;

namespace Popeye.Modules.ValueStatSystem
{
    public class TimeStaminaSystem : ATimeValueStat
    {
        private StaminaSystem _staminaSystem;

        private ITimeStaminaConfig _config;

        public bool ExhaustedAllStamina { get; private set; }
        private bool _isWaitingToRestoreStamina;
        private bool _isRestoringStamina;
        
        private CancellationTokenSource _waitToRestoreCancellation;
        private CancellationTokenSource _restoringCancellation;

        private Timer _fullRecoverTimer;
        
        public float FullRecoverDuration => _config.FullRecoverDuration;

        
        private float _temporaryRestoreT;
        private int TemporaryRestoredStamina => (int)(_temporaryRestoreT * _staminaSystem.MaxStamina);
        private bool HasTemporaryRestoredStamina => TemporaryRestoredStamina > 0;
        
        
        public TimeStaminaSystem(ITimeStaminaConfig config)
        {
            _config = config;
            Init();
        }

        private void Init()
        {
            _staminaSystem = new StaminaSystem(_config);
            ExhaustedAllStamina = _config.SpawnStamina == 0;
            _isWaitingToRestoreStamina = false;
            _isRestoringStamina = false;

            _fullRecoverTimer = new Timer(FullRecoverDuration);
        }
        
        
        public void Spend(int spendAmount)
        {
            if (_isRestoringStamina)
            {
                CancelRestoringStamina();
            }
            
            _staminaSystem.Spend(spendAmount);

            if (_staminaSystem.HasStaminaLeft())
            {
                StartRecoverAfterUse();
            }
            else
            {
                StartRecoverAfterExhaust();   
            }
    
            InvokeOnValueUpdate();
        }
    
        public void SpendAll()
        {
            if (_isRestoringStamina)
            {
                CancelRestoringStamina();
            }
            
            _staminaSystem.SpendAll();
            
            StartRecoverAfterExhaust();
    
            InvokeOnValueUpdate();
        }
    
    
        public void Restore(int gainAmount)
        {
            _staminaSystem.Restore(gainAmount);
    
            //InvokeOnValueUpdate();
        }
    
        public void RestoreAll()
        {
            _staminaSystem.RestoreAll();
    
            InvokeOnValueUpdate();
        }
    
        public bool HasStaminaLeft()
        {
            return _staminaSystem.HasStaminaLeft() || HasTemporaryRestoredStamina; 
        }
        public bool HasMaxStamina()
        {
            return _staminaSystem.HasMaxStamina();
        }
        public bool HasEnoughStamina(float staminaAmount)
        {
            return _staminaSystem.HasEnoughStamina(staminaAmount);
        }
        

        public override float GetValuePer1Ratio()
        {
            return _staminaSystem.GetValuePer1Ratio();
        }


        
        private void StartRecoverAfterUse()
        {
            WaitToRestore(_config.RecoverDelayAfterUseDuration);
        }
        private void StartRecoverAfterExhaust()
        {
            WaitToRestore(_config.RecoverDelayAfterExhaustDuration);
            ExhaustedAllStamina = true;
        }
        
        private void CancelWaitToRestoreStamina()
        {
            _waitToRestoreCancellation.Cancel();
        }
        private void CancelRestoringStamina()
        {
            RestoreTemporarilyRestoredStamina();
            _isRestoringStamina = false;
            _restoringCancellation.Cancel();
            
            InvokeOnValueStopUpdate();
        }


        private void WaitToRestore(float waitDuration)
        {
            /*
            if (_isRestoringStamina)
            {
                CancelRestoringStamina();
            }
            */
            
            if (_isWaitingToRestoreStamina)
            {
                CancelWaitToRestoreStamina();
            }
            
            _waitToRestoreCancellation = new CancellationTokenSource();
            DoWaitToRestore(waitDuration).Forget();
        }
        private async UniTaskVoid DoWaitToRestore(float waitDuration)
        {
            _isWaitingToRestoreStamina = true;
            
            await UniTask.Delay(TimeSpan.FromSeconds(waitDuration), 
                cancellationToken: _waitToRestoreCancellation.Token);
            
            _isWaitingToRestoreStamina = false;

            _restoringCancellation = new CancellationTokenSource();
            StartRestoring();
        }

        
        private void StartRestoring()
        {
            /*
            if (_isRestoringStamina)
            {
                CancelRestoringStamina();
            }
            */
            
            float durationToFullyRecover = ComputeDurationToFullyRecover();
            DoStartRestoring(durationToFullyRecover).Forget();
            InvokeOnValueStartUpdate(durationToFullyRecover);
        }
        private async UniTaskVoid DoStartRestoring(float durationToFullyRecover)
        {
            _isRestoringStamina = true;
            
            _fullRecoverTimer.SetDuration(durationToFullyRecover);
            _fullRecoverTimer.Clear();

            _temporaryRestoreT = 0f;
            
            while (!_fullRecoverTimer.HasFinished() && _isRestoringStamina)
            {
                float timeStep = Time.deltaTime;

                _temporaryRestoreT += timeStep / FullRecoverDuration;
                
                _fullRecoverTimer.Update(timeStep);
                await UniTask.Yield();
            }

            if (HasTemporaryRestoredStamina)
            {
                RestoreTemporarilyRestoredStamina();
            }
            
            _isRestoringStamina = false;
            ExhaustedAllStamina = false;
            
            _restoringCancellation.Dispose();
            _restoringCancellation = null;
        }

        private float ComputeDurationToFullyRecover()
        {
            return (1f - _staminaSystem.GetValuePer1Ratio()) * FullRecoverDuration;
        }

        private void RestoreTemporarilyRestoredStamina()
        {
            int restoredStamina = (int)(_temporaryRestoreT * _staminaSystem.MaxStamina);
            _staminaSystem.Restore(restoredStamina);
            _temporaryRestoreT = 0f;
        }
        
    }
}