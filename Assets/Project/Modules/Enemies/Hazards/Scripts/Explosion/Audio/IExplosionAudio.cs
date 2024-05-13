using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public interface IExplosionAudio
    {
        void PlayExplosionSound(GameObject source);
        void PlayDealDamageSound(GameObject source);
    }
}