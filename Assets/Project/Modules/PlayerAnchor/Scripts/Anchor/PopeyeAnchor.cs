

using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class PopeyeAnchor : MonoBehaviour, IAnchorMediator
    {
        private AnchorThrowTrajectory _anchorThrowTrajectory;


        public void Configure(AnchorThrowTrajectory anchorThrowTrajectory)
        {
            _anchorThrowTrajectory = anchorThrowTrajectory;
        }
        
        
        
        public void CancelChargingThrow()
        {
            // TODO
        }

        public void ResetThrowForce()
        {
            _anchorThrowTrajectory.ResetThrowForce();
        }

        public void IncrementThrowForce(float deltaTime)
        {
            _anchorThrowTrajectory.IncrementThrowForce(deltaTime);
        }

        public bool IsBeingThrown()
        {
            return _anchorThrowTrajectory.AnchorIsBeingThrown;
        }

        
        public void GetThrown(Vector3 throwDirection)
        {
            _anchorThrowTrajectory.ThrowAnchor(throwDirection).Forget();
        }

    }
}