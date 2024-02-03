
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public interface IChainView
    {
        void OnViewEnter();
        void LateUpdate(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition);
        void OnViewExit();
    }
}