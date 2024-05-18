namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IAnchorVerticalThrower
    {
        public PlayerMovesetActions ActionName { get; }
        public void ThrowAnchorVertically(out float duration);
    }
}