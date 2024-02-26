using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public interface IFMODAudioManager
    {
        void PlayOneShot(IOneShotFMODSound oneShotSound);
        void PlayOneShotAttached(IOneShotFMODSound oneShotSound, GameObject attachedGameObject);
        
        
        void PlayLastingSound(ILastingFMODSound lastingSound, GameObject attachedGameObject);
        void StopLastingSound(ILastingFMODSound lastingSound);

        void StopAllSounds();
    }
}