namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorViewExtras
    {
        void ResetView();
        void OnVerticalHit();
        void OnThrown();
        void OnPulled();
        void OnRestingOnFloor();
        void OnCarried();
    }
}