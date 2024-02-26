using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public class FMODAudioManager : IFMODAudioManager
    {
        private readonly OneShotSoundsController _oneShotSoundsController;
        private readonly LastingSoundsController _lastingSoundsController;
        public GlobalParametersController GlobalParametersController { get; private set; }

        public FMODAudioManager(OneShotSoundsController oneShotSoundsController, 
            LastingSoundsController lastingSoundsController,
            GlobalParametersController globalParametersController)
        {
            _oneShotSoundsController = oneShotSoundsController;
            _lastingSoundsController = lastingSoundsController;
            GlobalParametersController = globalParametersController;
        }


        public void PlayOneShot(OneShotFMODSound oneShotSound)
        {
            _oneShotSoundsController.Play(oneShotSound);
        }

        public void PlayOneShotAttached(OneShotFMODSound oneShotSound, GameObject attachedGameObject)
        {
            _oneShotSoundsController.Play(oneShotSound, attachedGameObject);
        }

        
        
        public void PlayLastingSound(LastingFMODSound lastingSound, GameObject attachedGameObject)
        {
            _lastingSoundsController.Play(lastingSound, attachedGameObject.transform);
        }

        public void StopLastingSound(LastingFMODSound lastingSound)
        {
            _lastingSoundsController.Stop(lastingSound);
        }
        

        
        public void StopAllSounds()
        {
            _lastingSoundsController.StopAll();
        }
    }
}