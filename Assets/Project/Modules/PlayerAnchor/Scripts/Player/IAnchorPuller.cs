namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IAnchorPuller
    {
        public bool AnchorIsBeingPulled();
        public void PullAnchor();
    }
}