using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public interface IFMODAudioManager
    {
        void PlayOneShot(FMODUnity.EventReference eventReference);
        void PlayOneShotAttached(FMODUnity.EventReference eventReference, GameObject attachedGameObject);
        
        
        void PlayLastingSound(ILastingFMODSound lastingSound, GameObject attachedGameObject);
        void StopLastingSound(ILastingFMODSound lastingSound);

        void StopAllSounds();
    }
}