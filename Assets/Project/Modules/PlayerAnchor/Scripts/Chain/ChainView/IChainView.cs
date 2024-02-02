
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public interface IChainView
    {
        void LateUpdate(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition);

        void OnViewSwapped();
    }
}