using Cysharp.Threading.Tasks;
using Popeye.Modules.ValueStatSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.Stamina
{
    public class PlayerStaminaSystem : IStaminaSystem, IPlayerStaminaPower
    {
        private readonly PlayerStaminaSystemConfig _config;
        private readonly TimeStepsStaminaSystem _baseStamina;
        private readonly TimeStepsStaminaSystem _extraStamina;
        
        public TimeStepsStaminaSystem BaseStamina => _baseStamina;
        public TimeStepsStaminaSystem ExtraStamina => _extraStamina;

        private bool ExtraStaminaIsActive => _extraStamina.HasStaminaLeft();
        
        
        public PlayerStaminaSystem(PlayerStaminaSystemConfig config)
        {
            _config = config;
            _baseStamina = new TimeStepsStaminaSystem(_config.BaseStaminaConfig);
            _extraStamina = new TimeStepsStaminaSystem(_config.ExtraStaminaConfig);

            _baseStamina.OnValueStepRestored += OnBaseStaminaRestored;
        }

        
        ~PlayerStaminaSystem() 
        {
            _baseStamina.OnValueStepRestored -= OnBaseStaminaRestored;
        }
        
        
        public bool HasMaxStamina()
        {
            if (ExtraStaminaIsActive)
            {
                return _extraStamina.HasMaxStamina();
            }
            else
            {
                return _baseStamina.HasStaminaLeft();
            }
        }

        public bool HasStaminaLeft()
        {
            return _baseStamina.HasStaminaLeft();
        }

        public void Spend(int spendAmount)
        {
            if (_extraStamina.HasStaminaLeft())
            {
                _extraStamina.Spend(spendAmount);
            }
            else
            {
                if (_baseStamina.HasMaxStamina())
                {
                    _config.ExtraStaminaConfig.OverwriteDelayStartRecoveringAfterExhausted(_config.BaseStaminaConfig.DelayRecoveringStep);
                    _extraStamina.CancelRestoring();   
                }
                
                _baseStamina.Spend(spendAmount);
            }
        }


        
        
        public void AddExtraStamina(int staminaAddAmount)
        {
            _extraStamina.ResetMaxValue(_extraStamina.MaxValue + staminaAddAmount, true);
        }

        public void RemoveExtraBoosts(int staminaRemoveAmount)
        {
            _extraStamina.ResetMaxValue(_extraStamina.MaxValue - staminaRemoveAmount, true);
        }



        private void OnBaseStaminaRestored()
        {
            if (_baseStamina.HasMaxStamina())
            {
                _extraStamina.StartRestoring();
                _config.ExtraStaminaConfig.ResetDelayStartRecoveringAfterExhausted();
            }
        }
        
        
    }
    
    
}