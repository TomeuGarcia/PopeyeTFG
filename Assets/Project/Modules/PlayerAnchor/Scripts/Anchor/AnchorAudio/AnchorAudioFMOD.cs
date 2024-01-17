using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class AnchorAudioFMOD : MonoBehaviour, IAnchorAudio
    {
        [SerializeField] private FMODUnity.EventReference _SFX_Hit_Anchor;
        private GameObject _anchorGameObject;

        [SerializeField] private FMODUnity.EventReference _SFX_Throw_Anchor;

        [SerializeField] private FMODUnity.EventReference _SFX_Grab_Anchor;


        public void Configure(GameObject anchorGameObject)
        {
            _anchorGameObject = anchorGameObject;
        }
        
        
        public void PlayThrowSound()
        {
            PlayOneShot(_SFX_Throw_Anchor);
           
        }

        public void PlayPickedUpSound()
        {
            PlayOneShot(_SFX_Grab_Anchor);
           
        }

        public void PlayDealDamageSound()
        {
            PlayOneShot(_SFX_Hit_Anchor);
          
        }


        private void PlayOneShot(FMODUnity.EventReference soundEventReference)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(soundEventReference, _anchorGameObject);
        }
        
    }
}