using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorSnapTarget : IAnchorTrajectorySnapTarget
    {
        Vector3 GetLookDirection();
    }
}