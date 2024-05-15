using UnityEngine;
using UnityEngine.InputSystem;

namespace Popeye.Modules.PlayerController.Inputs
{
    public class CameraAxisMovementInput : IMovementInputHandler
    {
        private readonly Transform _cameraTransform;
        private readonly InputSystem.PlayerAnchorInputControls _playerInputControls;
        private readonly InputAction _movementInputAction;
        private readonly InputAction _lookInputAction;

        public Vector3 ForwardAxis { get; private set; }
        public Vector3 RightAxis { get; private set; }

        public CameraAxisMovementInput(Transform cameraTransform)
        {
            _cameraTransform = cameraTransform;

            _playerInputControls = new InputSystem.PlayerAnchorInputControls();
            _playerInputControls.Enable();
            _movementInputAction = _playerInputControls.Land.Move;
            _lookInputAction = _playerInputControls.Land.Look;

            UpdateMovementAxis();
        }

        ~CameraAxisMovementInput()
        {
            _playerInputControls.Disable();
        }


        public Vector3 GetMovementInput()
        {
            Vector2 movementInput = _movementInputAction.ReadValue<Vector2>();

            return ToCameraAlignedInput(movementInput);
        }

        public Vector3 GetLookInput()
        {
            Vector3 lookInput = _lookInputAction.ReadValue<Vector2>();

            return ToCameraAlignedInput(lookInput);
        }


        private Vector3 ToCameraAlignedInput(Vector2 input)
        {
            UpdateMovementAxis();
            
            input = Vector2.ClampMagnitude(input, 1.0f);
            Vector3 result = Vector3.zero;
            
            result += RightAxis * input.x;
            result += ForwardAxis * input.y;

            return result;
        }

        private void UpdateMovementAxis()
        {
            RightAxis = _cameraTransform.right;
            ForwardAxis = Vector3.Cross(RightAxis, Vector3.up).normalized;
        }

        public void EnableMovement()
        {
            _movementInputAction.Enable();
        }

        public void DisableMovement()
        {
            _movementInputAction.Disable();
        }
    }
}