using Popeye.Core.Services.EventSystem;
using Popeye.Modules.GameState;
using Popeye.Scripts.ValueGating;
using UnityEngine.InputSystem;

namespace Popeye.Modules.PlayerController.Inputs
{
    public class PlayerAnchorMovesetInputsController
    {
        private readonly InputSystem.PlayerAnchorInputControls _playerInputControls;

        private readonly InputAction _aim;
        private readonly InputAction _cancelAim;
        private readonly InputAction _throw;
        
        private readonly InputAction _pickUp;
        
        private readonly InputAction _pull;

        private readonly IGateValueReader<InputAction> _pullGateValue;
        private readonly IGateValueReader<InputAction> _dashGateValue;
        private readonly IGateValueReader<InputAction> _specialAttackGateValue;
        private readonly InputAction _kick;
        
        private readonly InputAction _heal;
        
        private readonly InputAction _spinAttack_Left;
        private readonly InputAction _spinAttack_Right;


        private readonly IEventSystemService _eventSystemService;
        
        public PlayerAnchorMovesetInputsController(
            IEventSystemService eventSystemService,
            InputSystem.PlayerAnchorInputControls playerInputControls,
            IGateValueReader<InputAction> pullGateValue,
            IGateValueReader<InputAction> dashGateValue,
            IGateValueReader<InputAction> specialAttackGateValue
            )
        {
            _eventSystemService = eventSystemService;
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnGamePaused>(OnGamePausedEvent);
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnGameResumed>(OnGameResumedEvent);


            _playerInputControls = playerInputControls;
            EnabledInputs();
            

            _aim = _playerInputControls.Land.Aim;
            _cancelAim = _playerInputControls.Land.CancelAim;
            
            _throw = _playerInputControls.Land.Throw;
            
            _pickUp = _playerInputControls.Land.PickUp;

            _pullGateValue = pullGateValue;
            _dashGateValue = dashGateValue;
            _specialAttackGateValue = specialAttackGateValue;
            
            _kick = _playerInputControls.Land.Kick;

            _heal = _playerInputControls.Land.Heal;
            
            _spinAttack_Left = _playerInputControls.Land.SpinAttack_Left;
            _spinAttack_Right = _playerInputControls.Land.SpinAttack_Right;
        }

        ~PlayerAnchorMovesetInputsController()
        {
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnGamePaused>(OnGamePausedEvent);
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnGameResumed>(OnGameResumedEvent);
            
            DisabledInputs();
        }

        private void OnGamePausedEvent(IGameStateEventsDispatcher.OnGamePaused eventData)
        {
            DisabledInputs();
        }
        private void OnGameResumedEvent(IGameStateEventsDispatcher.OnGameResumed onGamePaused)
        {
            EnabledInputs();
        }
        
        private void EnabledInputs()
        {
            _playerInputControls.Enable();
        }
        
        private void DisabledInputs()
        {
            _playerInputControls.Disable();
        }
        
        

        public bool Aim_Pressed()
        {
            return _aim.WasPressedThisFrame();
        }
        public bool Aim_HeldPressed()
        {
            return _aim.IsPressed();
        }
        public bool Aim_Released()
        {
            return _aim.WasReleasedThisFrame();
        }
        
        
        public bool CancelAim_Pressed()
        {
            return _cancelAim.WasReleasedThisFrame();
            //return _cancelAim.WasPressedThisFrame();
        }
        
        
        public bool Throw_Pressed()
        {
            return _throw.WasPressedThisFrame();
        }
        public bool Throw_HeldPressed()
        {
            return _throw.IsPressed();
        }
        public bool Throw_Released()
        {
            return _throw.WasReleasedThisFrame();
        }
        
        
        public bool PickUp_Pressed()
        {
            return _pickUp.WasPressedThisFrame();
        }
        
        public bool Pull_Pressed()
        {
            return _pull.WasPressedThisFrame();
        }


        public bool Dash_Pressed()
        {
            return _dashGateValue.GetValue().WasPressedThisFrame();
        }
        
        
        public bool Kick_Pressed()
        {
            return _kick.WasPressedThisFrame();
        }
        
        
        public bool Heal_Pressed()
        {
            return _heal.WasPressedThisFrame();
        }
        public bool Heal_HeldPressed()
        {
            return _heal.IsPressed();
        }
        public bool Heal_Released()
        {
            return _heal.WasReleasedThisFrame();
        }
        
        public bool SpecialAttack_Pressed()
        {
            return _specialAttackGateValue.GetValue().WasPressedThisFrame();
        }
        public bool SpecialAttack_HeldPressed()
        {
            return _specialAttackGateValue.GetValue().IsPressed();
        }
        public bool SpecialAttack_Released()
        {
            return _specialAttackGateValue.GetValue().WasReleasedThisFrame();
        }
        
        
        public bool SpinAttack_Pressed(out bool spinRight)
        {
            spinRight = _spinAttack_Right.WasPressedThisFrame();
            return spinRight || _spinAttack_Left.WasPressedThisFrame();
        }
        public bool SpinAttack_HeldPressed()
        {
            return _spinAttack_Left.IsPressed() || _spinAttack_Right.IsPressed();
        }
        public bool SpinAttack_Released()
        {
            return !_spinAttack_Left.IsPressed() && !_spinAttack_Right.IsPressed();
        }
    }
}