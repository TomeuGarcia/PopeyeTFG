using Popeye.Modules.PlayerAnchor.Anchor;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IAnchorThrower
    {
        public bool AnchorIsBeingThrown();
        public void ThrowAnchor();
        public void ThrowAnchorVertically(out float duration);
        public void CancelChargingThrow();
        public void ResetThrowForce();
        public void IncrementThrowForce(float deltaTime);
        public AnchorThrowResult GetLastAnchorThrowResult();
    }
}