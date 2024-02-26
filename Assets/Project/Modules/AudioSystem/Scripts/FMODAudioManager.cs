using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public class FMODAudioManager : IFMODAudioManager
    {
        private readonly OneShotSoundsController _oneShotSoundsController;
        private readonly LastingSoundsController _lastingSoundsController;
        
        public FMODAudioManager(Transform soundsParent, LastingSoundsControllerConfig lastingSoundsControllerConfig)
        {
            _oneShotSoundsController = new OneShotSoundsController();
            _lastingSoundsController = new LastingSoundsController(soundsParent, lastingSoundsControllerConfig);
        }


        public void PlayOneShot(IOneShotFMODSound oneShotSound)
        {
            _oneShotSoundsController.Play(oneShotSound);
        }

        public void PlayOneShotAttached(IOneShotFMODSound oneShotSound, GameObject attachedGameObject)
        {
            _oneShotSoundsController.Play(oneShotSound, attachedGameObject);
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