namespace Popeye.Modules.VFX.ParticleFactories
{
    public interface IParticleFactory
    {
        void CreateEnemyOnHitParticles();
        void CreateEnemyOnHitSimpleParticle();
        void CreateEnemyOnHitCircleParticle();
    }
}