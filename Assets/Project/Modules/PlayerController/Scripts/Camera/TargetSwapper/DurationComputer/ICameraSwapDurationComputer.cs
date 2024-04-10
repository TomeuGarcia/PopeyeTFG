using UnityEngine;

namespace Popeye.Modules.Camera.TargetSwapper
{
    public interface ICameraSwapDurationComputer
    {
        float ComputeDuration(Transform originalFollowTarget, Transform newTarget);
    }
}