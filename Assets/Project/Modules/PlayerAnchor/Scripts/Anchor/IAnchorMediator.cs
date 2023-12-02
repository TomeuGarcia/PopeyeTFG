
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorMediator
    {
        public bool IsBeingThrown();
        public bool IsBeingPulled();
        public bool IsRestingOnFloor();

        public void OnCollisionWithObstacle(Collision collision);
        
        
    }
}