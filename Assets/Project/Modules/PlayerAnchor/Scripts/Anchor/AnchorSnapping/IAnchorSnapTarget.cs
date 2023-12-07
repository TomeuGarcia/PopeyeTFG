using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorSnapTarget : IAutoAimTarget
    {
        public Vector3 GetLookDirection();
    }
}