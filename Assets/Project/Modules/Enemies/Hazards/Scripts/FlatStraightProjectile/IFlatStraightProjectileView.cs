using Popeye.Modules.VFX.ParticleFactories;

namespace Popeye.Modules.Enemies.Hazards
{
    public interface IFlatStraightProjectileView
    {
        void Configure(IParticleFactory particleFactory);
        void ResetView();
        void PlayStartShootAnimation();
        void PlayHitObjectAnimation();
        void PlayDisappearAnimation(float duration);
    }
}