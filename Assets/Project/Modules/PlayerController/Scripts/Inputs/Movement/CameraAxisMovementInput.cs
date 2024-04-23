using UnityEngine;

namespace Popeye.Modules.PlayerController.Inputs
{
    public class CameraAxisMovementInput : IMovementInputHandler
    {
        private Transform _cameraTransform;
        private InputSystem.PlayerAnchorInputControls _playerInputControls;

        public Vector3 ForwardAxis { get; private set; }
        public Vector3 RightAxis { get; private set; }

        public CameraAxisMovementInput(Transform cameraTransform)
        {
            _cameraTransform = cameraTransform;

            _playerInputControls = new InputSystem.PlayerAnchorInputControls();
            _playerInputControls.Enable();

            UpdateMovementAxis();
        }

        ~CameraAxisMovementInput()
        {
            _playerInputControls.Disable();
        }


        public Vector3 GetMovementInput()
        {
            Vector2 movementInput = _playerInputControls.Land.Move.ReadValue<Vector2>();

            return ToCameraAlignedInput(movementInput);
        }

        public Vector3 GetLookInput()
        {
            Vector3 lookInput = _playerInputControls.Land.Look.ReadValue<Vector2>();

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
    }
}