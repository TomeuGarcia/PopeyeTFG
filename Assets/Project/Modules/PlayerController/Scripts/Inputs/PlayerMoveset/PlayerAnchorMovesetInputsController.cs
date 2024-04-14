using Popeye.Core.Services.EventSystem;
using Popeye.Modules.GameState;
using Popeye.Scripts.ValueGating;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Popeye.Modules.PlayerController.Inputs
{
    public class PlayerAnchorMovesetInputsController : IInputsUpdater
    {
        private readonly InputSystem.PlayerAnchorInputControls _playerInputControls;

        private readonly InputAction _aim;
        private readonly InputAction _cancelAim;
        private readonly InputAction _throw;
        private readonly IInputBuffer[] _inputBuffers;
        private readonly InputPressedBuffer _throwInputBuffer;
        
        private readonly InputAction _pickUp;
        

        private readonly IGateValueReader<InputAction> _pullGateValue;
        private readonly IGateValueReader<InputAction> _dashTowardsAnchorGateValue;
        private readonly IGateValueReader<InputAction> _dashDroppingAnchorGateValue;
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
            IGateValueReader<InputAction> dashTowardsAnchorGateValue,
            IGateValueReader<InputAction> dashDroppingAnchorGateValue,
            IGateValueReader<InputAction> specialAttackGateValue
            )
        {
            _eventSystemService = eventSystemService;
            StartListeningToGameEvents();


            _playerInputControls = playerInputControls;
            EnableInputs();
            

            _aim = _playerInputControls.Land.Aim;
            _cancelAim = _playerInputControls.Land.CancelAim;
            
            _throw = _playerInputControls.Land.Throw;
            _throwInputBuffer = new InputPressedBuffer(_playerInputControls.Land.Throw, 0.2f);

            _inputBuffers = new[]
            {
                _throwInputBuffer
            };
            
            _pickUp = _playerInputControls.Land.PickUp;

            _pullGateValue = pullGateValue;
            _dashTowardsAnchorGateValue = dashTowardsAnchorGateValue;
            _dashDroppingAnchorGateValue = dashDroppingAnchorGateValue;
            _specialAttackGateValue = specialAttackGateValue;
            
            _kick = _playerInputControls.Land.Kick;

            _heal = _playerInputControls.Land.Heal;
            
            _spinAttack_Left = _playerInputControls.Land.SpinAttack_Left;
            _spinAttack_Right = _playerInputControls.Land.SpinAttack_Right;
        }

        ~PlayerAnchorMovesetInputsController()
        {
            StopListeningToGameEvents();            
            DisableInputs();
        }

        public void Update(float deltaTime)
        {
            foreach (IInputBuffer inputBuffer in _inputBuffers)
            {
                inputBuffer.Update(deltaTime);
            }
        }
        
        private void StartListeningToGameEvents()
        {
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnGamePaused>(OnGamePausedEvent);
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnGameResumed>(OnGameResumedEvent);
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnExitToMainMenu>(OnExitToMainMenuEvent);
            
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnStartLoadingAdditiveScene>(OnStartLoadingAdditiveSceneEvent);
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnFinishLoadingScenes>(OnOnFinishLoadingScenes);
        }
        private void StopListeningToGameEvents()
        {
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnGamePaused>(OnGamePausedEvent);
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnGameResumed>(OnGameResumedEvent);
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnExitToMainMenu>(OnExitToMainMenuEvent);
                        
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnStartLoadingAdditiveScene>(OnStartLoadingAdditiveSceneEvent);
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnFinishLoadingScenes>(OnOnFinishLoadingScenes);
        }
        

        private void OnGamePausedEvent(IGameStateEventsDispatcher.OnGamePaused eventData)
        {
            DisableInputs();
        }
        private void OnGameResumedEvent(IGameStateEventsDispatcher.OnGameResumed eventData)
        {
            EnableInputs();
        }
        private void OnExitToMainMenuEvent(IGameStateEventsDispatcher.OnExitToMainMenu eventData)
        {
            DisableInputs();
        }
        private void OnStartLoadingAdditiveSceneEvent(IGameStateEventsDispatcher.OnStartLoadingAdditiveScene eventData)
        {
            DisableInputs();
        }
        private void OnOnFinishLoadingScenes(IGameStateEventsDispatcher.OnFinishLoadingScenes eventData)
        {
            EnableInputs();
        }
        
        
        
        private void EnableInputs()
        {
            _playerInputControls.Enable();
        }
        
        private void DisableInputs()
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
            return _throwInputBuffer.WasPressed();  ////////////////////////////////////
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
            return _pullGateValue.GetValue().WasPressedThisFrame();
        }


        public bool DashTowardsAnchor_Pressed()
        {
            return _dashTowardsAnchorGateValue.GetValue().WasPressedThisFrame();
        }
        public bool DashDroppingAnchor_Pressed()
        {
            return _dashDroppingAnchorGateValue.GetValue().WasPressedThisFrame();
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