namespace Popeye.Modules.PlayerController.Inputs
{
    public class PlayerAnchorMovesetInputsController
    {
        private InputSystem.PlayerAnchorInputControls _playerInputControls;

        private UnityEngine.InputSystem.InputAction _aim;
        private UnityEngine.InputSystem.InputAction _throw;
        
        private UnityEngine.InputSystem.InputAction _pickUp;
        /*
        private UnityEngine.InputSystem.InputAction _meleeAttack;
        private UnityEngine.InputSystem.InputAction _melee2;
        private UnityEngine.InputSystem.InputAction _move;
        private UnityEngine.InputSystem.InputAction _pullAttack;
        private UnityEngine.InputSystem.InputAction _explosionAbility;
        private UnityEngine.InputSystem.InputAction _electricChainAbility;
        */
        
        public PlayerAnchorMovesetInputsController()
        {
            _playerInputControls = new InputSystem.PlayerAnchorInputControls();
            _playerInputControls.Enable();

            _aim = _playerInputControls.Land.Aim;
            _throw = _playerInputControls.Land.Throw;
            
            _pickUp = _playerInputControls.Land.PickUp;
        }

        ~PlayerAnchorMovesetInputsController()
        {
            _playerInputControls.Disable();
        }


        public bool Aim_Pressed()
        {
            return _aim.WasPressedThisFrame();
        }
        public bool Aim_Released()
        {
            return _aim.WasReleasedThisFrame();
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


    }
}