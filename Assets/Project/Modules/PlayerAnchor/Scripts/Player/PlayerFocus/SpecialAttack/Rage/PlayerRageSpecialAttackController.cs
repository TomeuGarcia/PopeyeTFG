using System;
using Cysharp.Threading.Tasks;


namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    public class PlayerRageSpecialAttackController : IPlayerSpecialAttackController
    {
        private readonly IPlayerFocusSpender _focusSpender;
        private readonly PlayerFocusAttackConfig _focusAttackConfig;
        private readonly IRageSpecialAttackToggleable[] _specialAttackToggleables;

        private bool _isBeingPerformed;
        
        public float PreparationDuration => 0;
        public PlayerMovesetActions Name => PlayerMovesetActions.Rage;
        public void OnPreparationStart(float durationToComplete)
        {
        }

        public void OnPreparationInterrupted()
        {
        }

        public PlayerRageSpecialAttackController(
            IPlayerFocusSpender focusSpender, 
            PlayerFocusAttackConfig focusAttackConfig,
            IRageSpecialAttackToggleable[] specialAttackToggleables
            )
        {
            _focusSpender = focusSpender;
            _focusAttackConfig = focusAttackConfig;            
            _specialAttackToggleables = specialAttackToggleables;
            
            foreach (IRageSpecialAttackToggleable specialAttackToggleable in _specialAttackToggleables)
            {
                specialAttackToggleable.SetDefaultMode();
            }
        }

        public bool CanDoSpecialAttack()
        {
            return _focusSpender.HasEnoughFocus(_focusAttackConfig.RequiredFocusToPerform) && 
                   !SpecialAttackIsBeingPerformed();
        }

        private bool SpecialAttackIsBeingPerformed()
        {
            return _isBeingPerformed;
        }

        public void StartSpecialAttack()
        {
            _focusSpender.SpendFocus(_focusAttackConfig.RequiredFocusToPerform);
            DoSpecialAttack().Forget();
        }

        public bool SpecialAttackHasFinished()
        {
            return true;
        }

        public void ForceStopSpecialAttack()
        {
            // No need to cancel anything here
        }

        private async UniTaskVoid DoSpecialAttack()
        {
            foreach (IRageSpecialAttackToggleable specialAttackToggleable in _specialAttackToggleables)
            {
                specialAttackToggleable.SetSpecialAttackMode();
            }
            _isBeingPerformed = true;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_focusAttackConfig.AttackDuration));
            
            foreach (IRageSpecialAttackToggleable specialAttackToggleable in _specialAttackToggleables)
            {
                specialAttackToggleable.SetDefaultMode();
            }
            _isBeingPerformed = false;
        }
    }
}