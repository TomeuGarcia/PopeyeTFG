using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerAudioFMOD : MonoBehaviour, IPlayerAudio
    {
        
        private GameObject _playerGameObject;
        
        
        public void Configure(GameObject playerGameObject)
        {
            _playerGameObject = playerGameObject;
        }
        
        public void StartPlayingStepsSounds()
        {
            // TODO
        }

        public void StopPlayingStepsSounds()
        {
            // TODO
        }
        
        
        private void PlayOneShot(FMODUnity.EventReference soundEventReference)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(soundEventReference, _playerGameObject);
        }
    }
}