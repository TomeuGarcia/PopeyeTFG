
namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IAnchorThrower
    {
        public bool AnchorIsBeingThrown();
        public void UpdateThrowTrajectory();
        public void ThrowAnchor();
        public void CancelThrow();
        public void StartThrow();
        public void FinishThrow();
    }
}