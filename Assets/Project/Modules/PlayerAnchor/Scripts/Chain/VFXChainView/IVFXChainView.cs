using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public interface IVFXChainView
    {
        void Update(Vector3[] chainPositions);
        void StartOriginAnimation(Vector3 chainPosition, float duration);
    }
}