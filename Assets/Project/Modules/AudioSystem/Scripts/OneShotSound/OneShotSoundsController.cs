using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public class OneShotSoundsController
    {
        public OneShotSoundsController()
        {
        
        }

        public void Play(OneShotFMODSound oneShotSound)
        {
            EventInstance eventInstance = FMODUnity.RuntimeManager.CreateInstance(oneShotSound.EventReference);
            DoPlay(oneShotSound, ref eventInstance);
        }
        public void Play(OneShotFMODSound oneShotSound, GameObject attachedGameObject)
        {
            EventInstance eventInstance = FMODUnity.RuntimeManager.CreateInstance(oneShotSound.EventReference);
            eventInstance.set3DAttributes(attachedGameObject.transform.To3DAttributes());
            DoPlay(oneShotSound, ref eventInstance);
        }

        
        private void DoPlay(OneShotFMODSound oneShotSound, ref EventInstance eventInstance)
        {
            foreach (SoundParameter parameter in oneShotSound.Parameters)
            {
                eventInstance.setParameterByName(parameter.Name, parameter.Value);
            }
            
            eventInstance.start();
            eventInstance.release();
        }
        
        
    }
}