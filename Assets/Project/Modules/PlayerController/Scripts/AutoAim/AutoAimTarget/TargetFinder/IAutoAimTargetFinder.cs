using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public interface IAutoAimTargetFinder
    {
        bool GetAutoAimTargetsData(out IAutoAimTarget[] targets);
    }
}