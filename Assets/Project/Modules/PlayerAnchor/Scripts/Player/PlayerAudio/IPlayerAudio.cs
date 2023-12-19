using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IPlayerAudio
    {
        void Configure(GameObject playerGameObject);
        void StartPlayingStepsSounds();
        void StopPlayingStepsSounds();
    }
}