using UnityEngine;

namespace Popeye.Modules.PlayerController.Inputs
{
    public class DefaultInputCorrector : IInputCorrector
    {
        public Vector3 CorrectLookInput(Vector3 lookInput, Vector3 forwardDirection, Vector3 rightDirection)
        {
            return lookInput;
        }
    }
}