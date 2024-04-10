using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public interface IAbilityUnlockerChristalAudio
    {
        void PlayHitSound(GameObject source);
        void PlayBreakSound(GameObject source);
        void PlayCollectedSound(GameObject source);
    }
}