using Cysharp.Threading.Tasks;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorView
    {
        void PlayThrownAnimation(float duration);
        UniTaskVoid PlayPulledAnimation(float duration);
        void PlayKickedAnimation(float duration);
        void PlayCarriedAnimation();
        void PlayRestOnFloorAnimation();
    }
}