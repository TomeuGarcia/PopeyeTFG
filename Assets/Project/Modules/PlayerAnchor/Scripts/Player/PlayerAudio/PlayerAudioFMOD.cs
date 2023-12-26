using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerAudioFMOD : MonoBehaviour, IPlayerAudio
    {
        [SerializeField] private FMODUnity.StudioEventEmitter _footstepsEventEmitter;
        private GameObject _playerGameObject;
        
        
        public void Configure(GameObject playerGameObject)
        {
            _playerGameObject = playerGameObject;
        }
        
        public void StartPlayingStepsSounds()
        {
            _footstepsEventEmitter.Play();
        }

        public void StopPlayingStepsSounds()
        {
            _footstepsEventEmitter.Stop();
        }
        
        
    }
}

