
namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IAnchorThrower
    {
        public void UpdateThrowTrajectory();
        public void ThrowAnchor();
        public void CancelThrow();
        public void StartThrow();
        public void FinishThrow();
    }
}