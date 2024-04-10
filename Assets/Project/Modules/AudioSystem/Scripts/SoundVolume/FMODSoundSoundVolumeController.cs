using Popeye.Modules.AudioSystem.SoundVolume;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    [CreateAssetMenu(fileName = "SoundVolumeController_NAME", 
        menuName = ScriptableObjectsHelper.SOUNDSYSTEM_ASSETS_PATH + "SoundVolumeController")]
    public class FMODSoundSoundVolumeController : ScriptableObject, ISoundVolumeController
    {
        [SerializeField] private string _vcaName;
        private FMOD.Studio.VCA _vca;

        private const string VCA_PATH_ROOT = "vca:/";


        public float CurrentVolume { get; private set; }

        public void SetVolume(float volumeValue01)
        {
            _vca.setVolume(volumeValue01);
            CurrentVolume = volumeValue01;
        }
        
        
        public void Init(float volumeValue01)
        {
            _vca = FMODUnity.RuntimeManager.GetVCA(VCA_PATH_ROOT + _vcaName);
            SetVolume(volumeValue01);
        }
        
    }
}