using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public interface IAutoAimTargetFilterer
    {
        bool IsValidTarget(IAutoAimTarget autoAimTarget, Vector3 targeterPosition, 
            Vector3 targeterForwardDirection, Vector3 targeterUpDirection);
    }
}