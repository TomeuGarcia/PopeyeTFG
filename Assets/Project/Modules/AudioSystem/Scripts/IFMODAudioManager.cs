using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public interface IFMODAudioManager
    {
        public GlobalParametersController GlobalParametersController { get; }
        
        
        void PlayOneShot(OneShotFMODSound oneShotSound);
        void PlayOneShotAttached(OneShotFMODSound oneShotSound, GameObject attachedGameObject);
        
        
        void PlayLastingSound(LastingFMODSound lastingSound, GameObject attachedGameObject);
        void StopLastingSound(LastingFMODSound lastingSound);

        void StopAllSounds();
        
        
    }
}