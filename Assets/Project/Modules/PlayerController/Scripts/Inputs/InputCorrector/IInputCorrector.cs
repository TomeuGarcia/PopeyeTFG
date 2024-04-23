using UnityEngine;

namespace Popeye.Modules.PlayerController.Inputs
{
    public interface IInputCorrector
    {
        Vector3 CorrectLookInput(Vector3 lookInput, Vector3 forwardDirection, Vector3 rightDirection);
    }
}