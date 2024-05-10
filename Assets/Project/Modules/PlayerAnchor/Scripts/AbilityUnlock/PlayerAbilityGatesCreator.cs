using System.Collections.Generic;
using InputSystem;
using Popeye.Modules.PlayerAnchor.Player;
using Popeye.Modules.PlayerController.Inputs;
using Popeye.Scripts.ValueGating;
using UnityEngine.InputSystem;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public class PlayerAbilityGatesCreator
    {
        private readonly PlayerAnchorInputControls _playerAnchorInputControls;
        private readonly PlayerUnlockableAbilitiesConfig _unlockableAbilitiesConfig;
        private readonly IAnchorVerticalThrower _dashAttackVerticalThrower;
        private readonly IAnchorVerticalThrower _dashDropVerticalThrower;

        private ValueGate<InputPressedBuffer> _pullInputGate;
        private ValueGate<InputPressedBuffer> _dashTowardsAnchorInputGate;
        private ValueGate<InputAction> _dashDroppingAnchorInputGate;
        private ValueGate<InputAction> _spinAttackInputGate;
        private ValueGate<InputAction> _spikesAttackInputGate;
        private ValueGate<IAnchorVerticalThrower> _dashDroppingAnchorThrowerGate;

        private readonly List<PlayerAbilityUnlockGroup> _abilitiesToUnlock;
        
        

        public PlayerAbilityGatesCreator(PlayerAnchorInputControls playerAnchorInputControls,
            PlayerUnlockableAbilitiesConfig unlockableAbilitiesConfig,
            IAnchorVerticalThrower dashAttackVerticalThrower,
            IAnchorVerticalThrower dashDropVerticalThrower)
        {
            _playerAnchorInputControls = playerAnchorInputControls;
            _unlockableAbilitiesConfig = unlockableAbilitiesConfig;
            _dashAttackVerticalThrower = dashAttackVerticalThrower;
            _dashDropVerticalThrower = dashDropVerticalThrower;

            _abilitiesToUnlock = new List<PlayerAbilityUnlockGroup>(4);
        }


        public void CreateGates(PlayerMovesetInputsConfig playerMovesetInputsConfig)
        {            
            CreateInputGate(
                out _pullInputGate,
                new InputPressedBuffer(_playerAnchorInputControls.Land.Pull, 
                    playerMovesetInputsConfig.PullInputBufferDuration),
                _unlockableAbilitiesConfig.AnchorPull
            );
            
            CreateInputGate(
                out _dashTowardsAnchorInputGate,
                new InputPressedBuffer(_playerAnchorInputControls.Land.Dash, 
                    playerMovesetInputsConfig.DashTowardsAnchorInputBufferDuration),
                _unlockableAbilitiesConfig.DashTowardsAnchor
            );
            
            CreateInputGate(
                out _dashDroppingAnchorInputGate,
                _playerAnchorInputControls.Land.Dash,
                _unlockableAbilitiesConfig.DashDroppingAnchor
            );
            
            CreateInputGate(
                out _spinAttackInputGate,
                _playerAnchorInputControls.Land.SpecialAttack_AnchorSpin,
                _unlockableAbilitiesConfig.AnchorSpinAttack
            );
            
            CreateInputGate(
                out _spikesAttackInputGate,
                _playerAnchorInputControls.Land.SpecialAttack_ChainSpikes,
                _unlockableAbilitiesConfig.ChainSpikesAttack
            );

            CreateAnchorVerticalThrowerGate(
                out _dashDroppingAnchorThrowerGate,
                _unlockableAbilitiesConfig.DashDroppingAnchorAttack
            );
        }


        private void CreateInputGate(out ValueGate<InputPressedBuffer> inputGate, InputPressedBuffer openValue, 
            PlayerUnlockableAbilitiesConfig.IChannelAndState channelAndState)
        {
            CreateGate<InputPressedBuffer>(
                out inputGate,
                openValue,
                 new InputPressedBuffer(_playerAnchorInputControls.Land.NullAction, 0f),
                channelAndState,
                out PlayerAbilityUnlockGroup abilityUnlockGroup
            );
        }
        private void CreateInputGate(out ValueGate<InputAction> inputGate, InputAction openValue, 
            PlayerUnlockableAbilitiesConfig.IChannelAndState channelAndState)
        {
            CreateGate<InputAction>(
                out inputGate,
                openValue,
                _playerAnchorInputControls.Land.NullAction,
                channelAndState,
                out PlayerAbilityUnlockGroup abilityUnlockGroup
            );
        }
        
        private void CreateAnchorVerticalThrowerGate(out ValueGate<IAnchorVerticalThrower> inputGate, 
            PlayerUnlockableAbilitiesConfig.IChannelAndState channelAndState)
        {
            CreateGate<IAnchorVerticalThrower>(
                out inputGate,
                _dashAttackVerticalThrower,
                _dashDropVerticalThrower,
                channelAndState,
                out PlayerAbilityUnlockGroup abilityUnlockGroup
            );
            abilityUnlockGroup.AddGateToggle(_dashDroppingAnchorInputGate);
        }
        
        private void CreateGate<T>(out ValueGate<T> valueGateGate, T openValue, T closedValue,
            PlayerUnlockableAbilitiesConfig.IChannelAndState channelAndState, 
            out PlayerAbilityUnlockGroup abilityUnlockGroup)
        {
            valueGateGate = new ValueGate<T>(
                openValue,
                closedValue,
                channelAndState.IsUnlocked
            );

            if (!channelAndState.IsUnlocked)
            {
                abilityUnlockGroup = new PlayerAbilityUnlockGroup(valueGateGate, channelAndState.Channel);
                _abilitiesToUnlock.Add(abilityUnlockGroup);
            }
            else
            {
                abilityUnlockGroup = null;
            }
        }


        public void GetReadInputs(
            out IGateValueReader<InputPressedBuffer> pullInputGate,
            out IGateValueReader<InputPressedBuffer> dashTowardsAnchorInputGate,
            out IGateValueReader<InputAction> dashDroppingAnchorInputGate,
            out IGateValueReader<InputAction> spinAttackInputGate,
            out IGateValueReader<InputAction> spikesAttackInput
        )
        {
            pullInputGate = _pullInputGate;
            dashTowardsAnchorInputGate = _dashTowardsAnchorInputGate;
            dashDroppingAnchorInputGate = _dashDroppingAnchorInputGate;
            spinAttackInputGate = _spinAttackInputGate;
            spikesAttackInput = _spikesAttackInputGate;
        }

        public void GetReadDashDroppingAnchorThrow(
            out IGateValueReader<IAnchorVerticalThrower> dashDroppingAnchorThrowerGate
        )
        {
            dashDroppingAnchorThrowerGate = _dashDroppingAnchorThrowerGate;
        }

        public PlayerAbilityUnlockGroup[] GetAbilitiesToUnlock()
        {
            return _abilitiesToUnlock.ToArray();
        }
        
    }
}