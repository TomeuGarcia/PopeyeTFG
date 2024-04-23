using Popeye.Modules.PlayerAnchor.DropShadow;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class AnchorViewExtras : IAnchorViewExtras
    {
        private readonly DropShadowBehaviour _dropShadow;

        public AnchorViewExtras(DropShadowBehaviour dropShadow)
        {
            _dropShadow = dropShadow;
        }
        
        public void ResetView()
        {
            _dropShadow.Hide();
        }

        public void OnVerticalHit()
        {
            _dropShadow.Show();
        }

        public void OnThrown()
        {
            _dropShadow.Show();
        }
        public void OnPulled()
        {
            _dropShadow.Show();
        }

        public void OnRestingOnFloor()
        {
            _dropShadow.Hide();
        }
        public void OnCarried()
        {
            _dropShadow.Hide();
        }
    }
}