using Popeye.Modules.AudioSystem.SoundVolume;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    [CreateAssetMenu(fileName = "SoundVolumeController_NAME", 
        menuName = ScriptableObjectsHelper.SOUNDSYSTEM_ASSETS_PATH + "SoundVolumeController")]
    public class FMODSoundSoundVolumeController : ScriptableObject, ISoundVolumeController
    {
        [SerializeField] [FMODUnity.BankRef] private string _bankReference;
        private FMOD.Studio.Bus _bus;
        

        public float CurrentVolume
        {
            get
            {
                return 1.0f;
                _bus.getVolume(out float volumeValue01);
                return volumeValue01;
            }
        }

        public void SetVolume(float volumeValue01)
        {
            return;
            _bus.setVolume(volumeValue01);
        }
        
        
        public void Init(float volumeValue01)
        {
            return;
            _bus = FMODUnity.RuntimeManager.GetBus(_bankReference);
            SetVolume(volumeValue01);
        }
        
    }
}