using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public interface IFMODAudioManager
    {
        public GlobalParametersController GlobalParametersController { get; }
        
        
        void PlayOneShot(OneShotFMODSound oneShotSound);
        void PlayOneShotAttached(OneShotFMODSound oneShotSound, GameObject attachedGameObject);
        void PlayOneShots(OneShotFMODSound[] oneShotSounds);
        void PlayOneShotsAttached(OneShotFMODSound[] oneShotSounds, GameObject attachedGameObject);
        
        
        void PlayLastingSound(LastingFMODSound lastingSound, GameObject attachedGameObject);
        void StopLastingSound(LastingFMODSound lastingSound);
        void PlayLastingSounds(LastingFMODSound[] lastingSounds, GameObject attachedGameObject);
        void StopLastingSounds(LastingFMODSound[] lastingSounds);

        
        void StopAllSounds();
        
        
    }
}