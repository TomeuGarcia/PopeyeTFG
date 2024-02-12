using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public abstract class ABoneChainChainViewState
    {
        public BoneChainChainViewStates NextState { get; protected set; }
        
        public void Enter()
        {
            NextState = BoneChainChainViewStates.None;
            DoEnter();
        }
        
        protected abstract void DoEnter();
        public abstract void Exit();
        public abstract bool Update(Vector3[] positions, float positionsDistance);
        
    }
}