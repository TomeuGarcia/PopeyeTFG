using Cysharp.Threading.Tasks;
using Popeye.Modules.VFX.ParticleFactories;

namespace Popeye.Modules.Enemies.Hazards
{
    public interface IFlatStraightProjectileView
    {
        void Configure(IParticleFactory particleFactory, FlatStraightProjectileViewConfig config);
        void ResetView();
        void PlayStartShootAnimation();
        UniTask PlayObjectContactAnimation();
        void PlayDisappearAnimation(float duration);
    }
}