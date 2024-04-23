using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public interface IAutoAimTarget
    {
        AutoAimTargetDataConfig DataConfig { get; }
        Vector3 Position { get; }
        GameObject GameObject { get; }

        bool CanBeAimedAt(Vector3 aimFromPosition);
    }
}