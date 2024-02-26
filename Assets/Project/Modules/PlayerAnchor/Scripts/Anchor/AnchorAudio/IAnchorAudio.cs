using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorAudio
    {
        void PlayThrowSound();
        void PlayPickedUpSound();
        void PlayDealDamageSound();
    }
}