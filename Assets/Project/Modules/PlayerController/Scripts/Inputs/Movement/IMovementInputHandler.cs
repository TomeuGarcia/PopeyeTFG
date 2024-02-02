using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.PlayerController.Inputs
{
    public interface IMovementInputHandler
    {
        Vector3 ForwardAxis { get;  }
        Vector3 RightAxis { get; }
        
        Vector3 GetMovementInput();
        Vector3 GetLookInput();
    }
}


