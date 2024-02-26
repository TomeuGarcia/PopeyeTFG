using FMODUnity;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public class FMODAudioManager : IFMODAudioManager
    {
        private readonly LastingSoundsController _lastingSoundsController;
        
        public FMODAudioManager(Transform soundsParent, LastingSoundsControllerConfig lastingSoundsControllerConfig)
        {
            _lastingSoundsController = new LastingSoundsController(soundsParent, lastingSoundsControllerConfig);
        }
        
        
        
        public void PlayOneShot(EventReference eventReference)
        {
            FMODUnity.RuntimeManager.PlayOneShot(eventReference);
        }

        public void PlayOneShotAttached(EventReference eventReference, GameObject attachedGameObject)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(eventReference, attachedGameObject);
        }
        
        

        public void PlayLastingSound(ILastingFMODSound lastingSound, GameObject attachedGameObject)
        {
            _lastingSoundsController.Play(lastingSound, attachedGameObject.transform);
        }

        public void StopLastingSound(ILastingFMODSound lastingSound)
        {
            _lastingSoundsController.Stop(lastingSound);
        }
        

        public void StopAllSounds()
        {
            _lastingSoundsController.StopAll();
        }
    }
}