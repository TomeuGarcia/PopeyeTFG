namespace Popeye.Modules.PlayerController.Inputs
{
    public class PlayerAnchorMovesetInputsController
    {
        private readonly InputSystem.PlayerAnchorInputControls _playerInputControls;

        private readonly UnityEngine.InputSystem.InputAction _aim;
        private readonly UnityEngine.InputSystem.InputAction _cancelAim;
        private readonly UnityEngine.InputSystem.InputAction _throw;
        
        private readonly UnityEngine.InputSystem.InputAction _pickUp;
        
        private readonly UnityEngine.InputSystem.InputAction _pull;
        
        private readonly UnityEngine.InputSystem.InputAction _dash;
        private readonly UnityEngine.InputSystem.InputAction _kick;
        
        private readonly UnityEngine.InputSystem.InputAction _heal;
        
        private readonly UnityEngine.InputSystem.InputAction _spinAttack_Left;
        private readonly UnityEngine.InputSystem.InputAction _spinAttack_Right;

        
        
        public PlayerAnchorMovesetInputsController()
        {
            _playerInputControls = new InputSystem.PlayerAnchorInputControls();
            _playerInputControls.Enable();

            _aim = _playerInputControls.Land.Aim;
            _cancelAim = _playerInputControls.Land.CancelAim;
            
            _throw = _playerInputControls.Land.Throw;
            
            _pickUp = _playerInputControls.Land.PickUp;
            
            _pull = _playerInputControls.Land.Pull;

            _dash = _playerInputControls.Land.Dash;
            
            _kick = _playerInputControls.Land.Kick;

            _heal = _playerInputControls.Land.Heal;
            
            _spinAttack_Left = _playerInputControls.Land.SpinAttack_Left;
            _spinAttack_Right = _playerInputControls.Land.SpinAttack_Right;
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
        
        
        public bool CancelAim_Pressed()
        {
            return _cancelAim.WasPressedThisFrame();
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
            return _dash.WasPressedThisFrame();
        }
        
        
        public bool Kick_Pressed()
        {
            return _kick.WasPressedThisFrame();
        }
        
        
        public bool Heal_Pressed()
        {
            return _heal.WasPressedThisFrame();
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
    }
}