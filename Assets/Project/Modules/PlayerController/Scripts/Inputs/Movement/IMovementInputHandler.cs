using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.PlayerController.Inputs
{
    public interface IMovementInputHandler
    {
        public Vector3 GetMovementInput();
        public Vector3 GetLookInput();
    }
}


