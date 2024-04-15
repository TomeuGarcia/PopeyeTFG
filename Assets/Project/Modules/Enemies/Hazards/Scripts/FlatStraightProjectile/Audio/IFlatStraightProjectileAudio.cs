using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public interface IFlatStraightProjectileAudio
    {
        void Configure();
        
        void PlayMovingSound(GameObject source);
        void StopMovingSound();
        
        void PlayObjectContactSound(GameObject source);
        void PlayLifetimeEndSound(GameObject source);
    }
}