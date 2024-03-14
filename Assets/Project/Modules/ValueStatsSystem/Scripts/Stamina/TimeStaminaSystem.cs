using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Timer = Popeye.Timers.Timer;

namespace Popeye.Modules.ValueStatSystem
{
    public class TimeStaminaSystem : ATimeValueStat, IStaminaSystem
    {        
        public override int MaxValue => _staminaSystem.MaxValue;
        
        private StaminaSystem _staminaSystem;

        private readonly ITimeStaminaConfig _config;

        public bool ExhaustedAllStamina { get; private set; }
        private bool _isWaitingToRestoreStamina;
        private bool _isRestoringStamina;
        
        private int _progressiveSpendPerSecond = 20;
        private bool _spendingProgressively;


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



        private void DoSpendStart()
        {
            if (_isRestoringStamina)
            {
                CancelRestoringStamina();
            }
        }

        private void DoSpendEnd()
        {
            if (_staminaSystem.HasStaminaLeft())
            {
                StartRecoverAfterUse();
            }
            else
            {
                StartRecoverAfterExhaust();
                InvokeOnValueExhausted();
            }
    
            InvokeOnValueUpdate();
        }
        
        public void Spend(int spendAmount)
        {
            DoSpendStart();
            _staminaSystem.Spend(spendAmount);
            DoSpendEnd();
        }

        public void SetProgressiveSpendPerSecond(int progressiveSpendPerSecond)
        {
            _progressiveSpendPerSecond = progressiveSpendPerSecond;
        }
        public async UniTask SpendProgressively()
        {
            _spendingProgressively = true;
            
            DoSpendStart();
            
            float accumulatedSpendAmount = 0.0f;
            while (_spendingProgressively && _staminaSystem.HasStaminaLeft())
            {
                while (accumulatedSpendAmount < 1)
                {
                    accumulatedSpendAmount += Time.deltaTime * _progressiveSpendPerSecond;
                    await UniTask.Yield();
                }
                
                int spendAmount = (int)accumulatedSpendAmount;
                accumulatedSpendAmount -= spendAmount;
                
                _staminaSystem.Spend(spendAmount);
                InvokeOnValueUpdate();
            }
            
            DoSpendEnd();
        }
        public void StopSpendingProgressively()
        {
            _spendingProgressively = false;
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
        public override int GetValue()
        {
            return _staminaSystem.GetValue();
        }

        protected override void DoResetMaxValue(int maxValue, bool setValueToMax)
        {
            _staminaSystem.ResetMaxValue(maxValue, setValueToMax);
        }


        private void StartRecoverAfterUse()
        {
            WaitToRestore(_config.RecoverAfterUseDelayDuration);
        }
        private void StartRecoverAfterExhaust()
        {
            WaitToRestore(_config.RecoverAfterExhaustDelayDuration);
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

            if (!_spendingProgressively || !HasStaminaLeft())
            {
                _spendingProgressively = false;
                _restoringCancellation = new CancellationTokenSource();
                StartRestoring();
            }
        }

        
        private void StartRestoring()
        {
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
            
            while (!_fullRecoverTimer.HasFinished() && _isRestoringStamina && !_spendingProgressively)
            {
                float timeStep = Time.deltaTime;

                _temporaryRestoreT += timeStep / FullRecoverDuration;
                
                _fullRecoverTimer.Update(timeStep);
                await UniTask.Yield();
            }

            if (HasTemporaryRestoredStamina && !_spendingProgressively)
            {
                RestoreTemporarilyRestoredStamina();
            }
            
            _isRestoringStamina = false;
            ExhaustedAllStamina = false;
            
            _restoringCancellation?.Dispose();
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