using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorAudio
    {
        void Configure(GameObject anchorGameObject);
        void PlayThrowSound();
        void PlayPickedUpSound();
        void PlayDealDamageSound();
    }
}