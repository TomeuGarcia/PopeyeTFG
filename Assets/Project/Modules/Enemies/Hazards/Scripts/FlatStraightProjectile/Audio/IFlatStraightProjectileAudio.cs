using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public interface IFlatStraightProjectileAudio
    {
        void PlayObjectContactSound(GameObject source);
        void PlayLifetimeEndSound(GameObject source);
        
        void PlayDealDamageSound(GameObject source);
    }
}