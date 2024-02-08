
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public interface IChainViewLogic
    {
        void OnViewEnter(Vector3[] previousStateChainPositions, Vector3 playerBindPosition, Vector3 anchorBindPosition);
        void UpdateChainPositions(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition);
        void OnViewExit();

        Vector3[] GetChainPositions();
    }
}