using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IPlayerAudio : IPlayerFootstepsListener
    {
        void StartPlayingStepsSounds();
        void StopPlayingStepsSounds();
        void PlayDashTowardsAnchorSound();
        void PlayDashDroppingAnchorSound();
        void PlayTakeDamageSound();
    }
}