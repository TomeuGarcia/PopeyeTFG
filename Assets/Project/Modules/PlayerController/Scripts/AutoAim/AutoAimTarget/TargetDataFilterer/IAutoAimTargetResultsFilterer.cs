using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public interface IAutoAimTargetResultsFilterer
    {
        AutoAimTargetResult[] Filter(AutoAimTargetResult[] targetsResults, Vector3 targeterPosition);
    }
}