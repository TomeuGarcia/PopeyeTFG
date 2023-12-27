using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorView
    {
        void ResetView();
        UniTaskVoid PlayVerticalHitAnimation(float duration, RaycastHit floorHit);
        void PlayThrownAnimation(float duration);
        UniTaskVoid PlayPulledAnimation(float duration);
        void PlayKickedAnimation(float duration);
        void PlayCarriedAnimation();
        void PlayRestOnFloorAnimation();
        
        void PlaySpinningAnimation();
        void PlayObstructedAnimation();
    }
}