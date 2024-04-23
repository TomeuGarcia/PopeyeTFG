using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Anchor;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    public class PlayerFocusSpecialAttackController : IPlayerSpecialAttackController
    {
        private readonly IPlayerFocusSpender _focusSpender;
        private readonly PlayerFocusAttackConfig _focusAttackConfig;
        private readonly ISpecialAttackToggleable[] _specialAttackToggleables;

        private bool _isBeingPerformed;
        
        public PlayerFocusSpecialAttackController(IPlayerFocusSpender focusSpender, PlayerFocusAttackConfig focusAttackConfig,
            ISpecialAttackToggleable[] specialAttackToggleables)
        {
            _focusSpender = focusSpender;
            _focusAttackConfig = focusAttackConfig;
            _specialAttackToggleables = specialAttackToggleables;
            
            foreach (ISpecialAttackToggleable specialAttackToggleable in _specialAttackToggleables)
            {
                specialAttackToggleable.SetDefaultMode();
            }
        }
        
        public bool CanDoSpecialAttack()
        {
            return _focusSpender.HasEnoughFocus(_focusAttackConfig.RequiredFocusToPerform) && 
                   !SpecialAttackIsBeingPerformed();
        }

        public bool SpecialAttackIsBeingPerformed()
        {
            return _isBeingPerformed;
        }

        public void StartSpecialAttack()
        {
            _focusSpender.SpendFocus(_focusAttackConfig.RequiredFocusToPerform);
            DoSpecialAttack().Forget();
        }
        
        private async UniTaskVoid DoSpecialAttack()
        {
            foreach (ISpecialAttackToggleable specialAttackToggleable in _specialAttackToggleables)
            {
                specialAttackToggleable.SetSpecialAttackMode();
            }
            _isBeingPerformed = true;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_focusAttackConfig.AttackDuration));
            
            foreach (ISpecialAttackToggleable specialAttackToggleable in _specialAttackToggleables)
            {
                specialAttackToggleable.SetDefaultMode();
            }
            _isBeingPerformed = false;
        }
    }
}