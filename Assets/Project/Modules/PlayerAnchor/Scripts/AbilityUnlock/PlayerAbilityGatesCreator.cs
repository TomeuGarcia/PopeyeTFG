using System.Collections.Generic;
using InputSystem;
using Popeye.Scripts.ValueGating;
using UnityEngine.InputSystem;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public class PlayerAbilityGatesCreator
    {
        private readonly PlayerAnchorInputControls _playerAnchorInputControls;
        private readonly PlayerUnlockableAbilitiesConfig _unlockableAbilitiesConfig;

        private ValueGate<InputAction> _pullInputGate;
        private ValueGate<InputAction> _dashTowardsAnchorInputGate;
        private ValueGate<InputAction> _dashDroppingAnchorInputGate;
        private ValueGate<InputAction> _specialAttackInputGate;

        private List<PlayerAbilityUnlockGroup> _abilitiesToUnlock;
        
        

        public PlayerAbilityGatesCreator(PlayerAnchorInputControls playerAnchorInputControls,
            PlayerUnlockableAbilitiesConfig unlockableAbilitiesConfig)
        {
            _playerAnchorInputControls = playerAnchorInputControls;
            _unlockableAbilitiesConfig = unlockableAbilitiesConfig;

            _abilitiesToUnlock = new List<PlayerAbilityUnlockGroup>(4);
        }


        public void CreateGates()
        {
            CreateInputGate(
                out _pullInputGate,
                _playerAnchorInputControls.Land.Pull,
                _unlockableAbilitiesConfig.AnchorPull
            );
            
            CreateInputGate(
                out _dashTowardsAnchorInputGate,
                _playerAnchorInputControls.Land.Dash,
                _unlockableAbilitiesConfig.DashTowardsAnchor
            );
            
            CreateInputGate(
                out _dashDroppingAnchorInputGate,
                _playerAnchorInputControls.Land.Dash,
                _unlockableAbilitiesConfig.DashDroppingAnchor
            );
            
            CreateInputGate(
                out _specialAttackInputGate,
                _playerAnchorInputControls.Land.SpecialAttack,
                _unlockableAbilitiesConfig.SpecialAttack
            );
        }


        private void CreateInputGate(out ValueGate<InputAction> inputGate, InputAction openValue, 
            PlayerUnlockableAbilitiesConfig.IChannelAndState channelAndState)
        {
            inputGate = new ValueGate<InputAction>(
                openValue,
                _playerAnchorInputControls.Land.NullAction,
                channelAndState.IsUnlocked
            );

            if (!channelAndState.IsUnlocked)
            {
                _abilitiesToUnlock.Add(new PlayerAbilityUnlockGroup(inputGate, channelAndState.Channel));
            }
        }
        

        public void GetReadInputs(
            out IGateValueReader<InputAction> pullInputGate,
            out IGateValueReader<InputAction> dashTowardsAnchorInputGate,
            out IGateValueReader<InputAction> dashDroppingAnchorInputGate,
            out IGateValueReader<InputAction> specialAttackInputGate
        )
        {
            pullInputGate = _pullInputGate;
            dashTowardsAnchorInputGate = _dashTowardsAnchorInputGate;
            dashDroppingAnchorInputGate = _dashDroppingAnchorInputGate;
            specialAttackInputGate = _specialAttackInputGate;
        }

        public PlayerAbilityUnlockGroup[] GetAbilitiesToUnlock()
        {
            return _abilitiesToUnlock.ToArray();
        }
        
    }
}