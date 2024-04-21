using Project.Modules.AudioSystem.Scripts.SoundVolume;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public abstract class AFMODAudioManagerReference : ScriptableObject, IFMODAudioManager
    {
        protected IFMODAudioManager _audioManager;

        public GlobalParametersController GlobalParametersController => _audioManager.GlobalParametersController;
        public SoundVolumeControllersGroup SoundVolumeControllersGroup => _audioManager.SoundVolumeControllersGroup;
        
        
        public void PlayOneShot(OneShotFMODSound oneShotSound)
        {
            _audioManager.PlayOneShot(oneShotSound);
        }

        public void PlayOneShotAttached(OneShotFMODSound oneShotSound, GameObject attachedGameObject)
        {
            _audioManager.PlayOneShotAttached(oneShotSound, attachedGameObject);
        }

        public void PlayOneShots(OneShotFMODSound[] oneShotSounds)
        {
            _audioManager.PlayOneShots(oneShotSounds);
        }

        public void PlayOneShotsAttached(OneShotFMODSound[] oneShotSounds, GameObject attachedGameObject)
        {
            _audioManager.PlayOneShotsAttached(oneShotSounds, attachedGameObject);
        }

        public void PlayLastingSound(LastingFMODSound lastingSound, GameObject attachedGameObject)
        {
            _audioManager.PlayLastingSound(lastingSound, attachedGameObject);
        }

        public void StopLastingSound(LastingFMODSound lastingSound)
        {
            _audioManager.StopLastingSound(lastingSound);
        }

        public void PlayLastingSounds(LastingFMODSound[] lastingSounds, GameObject attachedGameObject)
        {
            _audioManager.PlayLastingSounds(lastingSounds, attachedGameObject);
        }

        public void StopLastingSounds(LastingFMODSound[] lastingSounds)
        {
            _audioManager.StopLastingSounds(lastingSounds);
        }

        public void StopAllSounds()
        {
            _audioManager.StopAllSounds();
        }
    }
}