using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Anchor;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    public class PlayerFocusSpecialAttackController : IPlayerSpecialAttackController
    {
        private readonly IPlayerFocusSpender _focusSpender;
        private readonly PlayerFocusAttackConfig _focusAttackConfig;
        private readonly AnchorDamageConfig _anchorDamageConfig;

        private bool _isBeingPerformed;
        
        public PlayerFocusSpecialAttackController(IPlayerFocusSpender focusSpender, PlayerFocusAttackConfig focusAttackConfig,
            AnchorDamageConfig anchorDamageConfig)
        {
            _focusSpender = focusSpender;
            _focusAttackConfig = focusAttackConfig;
            _anchorDamageConfig = anchorDamageConfig;
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
            _anchorDamageConfig.SetEnraged();
            _isBeingPerformed = true;
            await UniTask.Delay(TimeSpan.FromSeconds(_focusAttackConfig.AttackDuration));
            _anchorDamageConfig.SetDefault();
            _isBeingPerformed = false;
        }
    }
}