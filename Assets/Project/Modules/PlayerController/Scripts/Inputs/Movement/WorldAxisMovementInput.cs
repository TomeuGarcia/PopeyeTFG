using InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.PlayerController.Inputs
{
    public class WorldAxisMovementInput : IMovementInputHandler
    {
        private InputSystem.PlayerAnchorInputControls _playerInputControls;

        public Vector3 ForwardAxis => Vector3.forward;
        public Vector3 RightAxis => Vector3.right;

        public WorldAxisMovementInput()
        {
            _playerInputControls = new InputSystem.PlayerAnchorInputControls();
            _playerInputControls.Enable();
        }

        ~WorldAxisMovementInput()
        {
            _playerInputControls.Disable();
        }


        public Vector3 GetMovementInput()
        {
            Vector2 movementInput = _playerInputControls.Land.Move.ReadValue<Vector2>();

            return ToWorldInput(movementInput);
        }

        public Vector3 GetLookInput()
        {
            Vector2 lookInput = _playerInputControls.Land.Look.ReadValue<Vector2>();

            return ToWorldInput(lookInput);
        }


        private Vector3 ToWorldInput(Vector2 input)
        {
            input = Vector2.ClampMagnitude(input, 1.0f);

            return new Vector3(input.x, 0.0f, input.y);
        }
    }
}