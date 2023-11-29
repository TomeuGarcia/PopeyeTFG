using Project.Modules.PlayerAnchor.Anchor;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IAnchorThrower
    {
        public void ThrowAnchor();
        public void CancelChargingThrow();
        public void ResetThrowForce();
        public void IncrementThrowForce(float deltaTime);
    }
}