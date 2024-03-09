using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IPlayerAudio
    {
        void StartPlayingStepsSounds();
        void StopPlayingStepsSounds();
        void PlayDashTowardsAnchorSound();
        void PlayDashDroppingAnchorSound();
        void PlayTakeDamageSound();
    }
}