using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorSnapTarget : IAutoAimTarget
    {
        public Vector3 GetLookDirection();
    }
}