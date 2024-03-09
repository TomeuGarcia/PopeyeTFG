using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    public class TimeStepsStaminaSystem : ATimeStepValueStat, IStaminaSystem
    {
        public override int MaxValue => _staminaSystem.MaxValue;
        private readonly TimeStepsStaminaSystemConfig _config;
        private readonly StaminaSystem _staminaSystem;

        private CancellationTokenSource _waitToStartRestoringCTS;
        private bool _isRestoringStamina;
        

        public TimeStepsStaminaSystem(TimeStepsStaminaSystemConfig config)
        {
            _config = config;
            _staminaSystem = new StaminaSystem(_config);
        }
        
        
        public override float GetValuePer1Ratio()
        {
            return _staminaSystem.GetValuePer1Ratio();
        }

        public override int GetValue()
        {
            return _staminaSystem.GetValue();
        }
        
        

        public bool HasMaxStamina()
        {
            return _staminaSystem.HasMaxStamina();
        }

        public bool HasStaminaLeft()
        {
            return _staminaSystem.HasStaminaLeft();
        }

        public void Spend(int spendAmount)
        {
            _staminaSystem.Spend(spendAmount);
            InvokeOnValueUpdate();

            if (_isRestoringStamina)
            {
                _waitToStartRestoringCTS.Cancel();
            }

            _waitToStartRestoringCTS = new CancellationTokenSource();
            StartRestoring(_waitToStartRestoringCTS).Forget();
        }


        private async UniTaskVoid StartRestoring(CancellationTokenSource cancellationTokenSource)
        {
            _isRestoringStamina = true;
            
            float startRestoringDelay = HasStaminaLeft()
                ? _config.DelayStartRecovering
                : _config.DelayStartRecoveringAfterExhausted;

            
            await UniTask.Delay(TimeSpan.FromSeconds(startRestoringDelay), 
                cancellationToken: cancellationTokenSource.Token);

            bool canRestoreMoreStamina = !HasMaxStamina();
            

            while (canRestoreMoreStamina && !cancellationTokenSource.IsCancellationRequested)
            {
                _staminaSystem.Restore(_config.StaminaAmountRecoveringStep);
                canRestoreMoreStamina = !HasMaxStamina();
                InvokeOnValueStepRestored();

                await UniTask.Delay(TimeSpan.FromSeconds(_config.DelayRecoveringStep),
                    cancellationToken: cancellationTokenSource.Token);
            }

            _isRestoringStamina = false;
            cancellationTokenSource.Dispose();
        }

        
        
    }
}