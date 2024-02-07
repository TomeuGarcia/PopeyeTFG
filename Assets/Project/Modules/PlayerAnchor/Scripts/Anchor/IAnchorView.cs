using Cysharp.Threading.Tasks;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.VFX.ParticleFactories;
using Project.Scripts.Time.TimeHitStop;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorView
    {
        void Configure(IParticleFactory particleFactory, IHitStopManager hitStopManager, ICameraShaker cameraShaker);
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
        void OnDamageDealt(DamageHitResult damageHitResult);
    }
}