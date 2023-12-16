
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorMediator
    {
        Vector3 Position { get; }
        
        public bool IsBeingThrown();
        public bool IsBeingPulled();
        public bool IsRestingOnFloor();

        public bool IsGrabbedBySnapper();

    }
}