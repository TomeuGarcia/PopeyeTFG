using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    public class TimeStaminaSystem : AValueStat
    {
        private StaminaSystem _staminaSystem;

        private ITimeStaminaConfig _config;

        public bool ExhaustedAllStamina { get; private set; }
        private bool _isWaitingToRestoreStamina;
        private bool _isRestoringStamina;
        
        private CancellationTokenSource _waitToRestoreCancellation;
        private CancellationTokenSource _restoringStaminaCancellation;

        public float FullRecoverDuration => _config.FullRecoverDuration;

        
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
        }
        
        
        public void Spend(int spendAmount)
        {
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
            _staminaSystem.SpendAll();
            
            StartRecoverAfterExhaust();
    
            InvokeOnValueUpdate();
        }
    
    
        public void Restore(int gainAmount)
        {
            _staminaSystem.Restore(gainAmount);
    
            InvokeOnValueUpdate();
        }
    
        public void RestoreAll()
        {
            _staminaSystem.RestoreAll();
    
            InvokeOnValueUpdate();
        }
    
        public bool HasStaminaLeft()
        {
            return _staminaSystem.HasStaminaLeft();
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
            _restoringStaminaCancellation.Cancel();
        }


        private void WaitToRestore(float waitDuration)
        {
            if (_isRestoringStamina)
            {
                CancelRestoringStamina();
            }
            
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
            
            StartRestoring();
        }


        
        private void StartRestoring()
        {
            if (_isRestoringStamina)
            {
                CancelRestoringStamina();
            }
            
            _restoringStaminaCancellation = new CancellationTokenSource();
            DoStartRestoring().Forget();
        }
        private async UniTaskVoid DoStartRestoring()
        {
            _isRestoringStamina = true;
            
            float staminaPerTick = _staminaSystem.MaxStamina / _config.FullRecoverDuration;
            
            while (!_staminaSystem.HasMaxStamina())
            {
                float timeSpan = Time.deltaTime;
                int restoredStaminaStep = (int)Mathf.Max(staminaPerTick * timeSpan, 1);
                Restore(restoredStaminaStep);

                await UniTask.Delay(TimeSpan.FromSeconds(timeSpan),
                    cancellationToken: _restoringStaminaCancellation.Token);
            }
            
            _restoringStaminaCancellation.Dispose();
            _restoringStaminaCancellation = null;

            _isRestoringStamina = false;
            ExhaustedAllStamina = false;
        }

    }
}