using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public class OneShotSoundsController
    {
        public OneShotSoundsController()
        {
        
        }

        public void Play(IOneShotFMODSound oneShotSound)
        {
            FMODUnity.RuntimeManager.PlayOneShot(oneShotSound.EventReference);
        }
        public void Play(IOneShotFMODSound oneShotSound, GameObject attachedGameObject)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(oneShotSound.EventReference, attachedGameObject);
        }
    }
}