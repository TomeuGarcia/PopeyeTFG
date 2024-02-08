using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public interface IChainView
    {
        public void Update(Vector3[] positions);
        public void DrawGizmos();
    }
}