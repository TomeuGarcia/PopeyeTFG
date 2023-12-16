using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorView
    {
        UniTaskVoid PlayVerticalHitAnimation(float duration, RaycastHit floorHit);
        void PlayThrownAnimation(float duration);
        UniTaskVoid PlayPulledAnimation(float duration);
        void PlayKickedAnimation(float duration);
        void PlayCarriedAnimation();
        void PlayRestOnFloorAnimation();
    }
}