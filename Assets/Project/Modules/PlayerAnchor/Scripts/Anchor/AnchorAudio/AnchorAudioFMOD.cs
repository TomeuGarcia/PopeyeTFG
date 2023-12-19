using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class AnchorAudioFMOD : MonoBehaviour, IAnchorAudio
    {
        [SerializeField] private FMODUnity.EventReference _exampleER;
        private GameObject _anchorGameObject;
        
        
        public void Configure(GameObject anchorGameObject)
        {
            _anchorGameObject = anchorGameObject;
        }
        
        
        public void PlayThrowSound()
        {
            // TODO
            // PlayOneShot(_exampleER);
        }

        public void PlayPickedUpSound()
        {
            // TODO
            // PlayOneShot();
        }

        public void PlayDealDamageSound()
        {
            // TODO
            // PlayOneShot();
        }


        private void PlayOneShot(FMODUnity.EventReference soundEventReference)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(soundEventReference, _anchorGameObject);
        }
        
    }
}