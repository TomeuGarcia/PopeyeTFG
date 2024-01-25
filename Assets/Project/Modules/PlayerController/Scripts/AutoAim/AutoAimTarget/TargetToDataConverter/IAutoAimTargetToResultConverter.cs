using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public interface IAutoAimTargetToResultConverter
    {
        AutoAimTargetResult[] Convert(IAutoAimTarget[] autoAimTargets, Vector3 forwardDirection, Vector3 rightDirection);
        AutoAimTargetResult Convert(IAutoAimTarget autoAimTarget, Vector3 forwardDirection, Vector3 rightDirection);
    }
}