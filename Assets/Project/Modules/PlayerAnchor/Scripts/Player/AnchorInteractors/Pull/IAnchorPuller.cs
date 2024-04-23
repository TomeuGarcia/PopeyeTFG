namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IAnchorPuller
    {
        public bool IsAutoQueued { get; }
        public bool AnchorIsBeingPulled();
        public void PullAnchor(bool isAutoQueued);
    }
}