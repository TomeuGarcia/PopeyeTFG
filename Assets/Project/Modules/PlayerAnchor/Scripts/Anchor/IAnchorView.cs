using Cysharp.Threading.Tasks;
using Popeye.Modules.VFX.ParticleFactories;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorView
    {
        void Configure(IParticleFactory particleFactory);
        void ResetView();
        UniTaskVoid PlayVerticalHitAnimation(float duration, RaycastHit floorHit);
        UniTaskVoid PlayThrownAnimation(float duration);
        UniTaskVoid PlayPulledAnimation(float duration);
        void PlayKickedAnimation(float duration);
        void PlayCarriedAnimation();
        void PlayRestOnFloorAnimation();
        
        void PlaySpinningAnimation();
        void PlayObstructedAnimation();
        void StopCarry();
    }
}