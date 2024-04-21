using Project.Modules.AudioSystem.Scripts.SoundVolume;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public interface IFMODAudioManager
    {
        public GlobalParametersController GlobalParametersController { get; }
        public SoundVolumeControllersGroup SoundVolumeControllersGroup { get; }
        
        
        void PlayOneShot(OneShotFMODSound oneShotSound);
        void PlayOneShotAttached(OneShotFMODSound oneShotSound, GameObject attachedGameObject);
        void PlayOneShots(OneShotFMODSound[] oneShotSounds);
        void PlayOneShotsAttached(OneShotFMODSound[] oneShotSounds, GameObject attachedGameObject);
        
        
        LastingFMODSound.SoundId PlayLastingSound(LastingFMODSound lastingSound, GameObject attachedGameObject);
        void StopLastingSound(LastingFMODSound.SoundId lastingSoundId);
        LastingFMODSound.SoundId[]  PlayLastingSounds(LastingFMODSound[] lastingSounds, GameObject attachedGameObject);
        void StopLastingSounds(LastingFMODSound.SoundId[] lastingSoundIds);

        
        void StopAllSounds();
        
        
    }
}