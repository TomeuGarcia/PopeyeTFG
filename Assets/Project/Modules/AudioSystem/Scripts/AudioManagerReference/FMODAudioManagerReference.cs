using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    [CreateAssetMenu(fileName = "FMODAudioManagerReference", 
        menuName = ScriptableObjectsHelper.SOUNDSYSTEM_ASSETS_PATH + "FMODAudioManagerReference")]
    public class FMODAudioManagerReference : AFMODAudioManagerReference, IFMODAudioConsumer
    {

        public void Configure(IFMODAudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        
    }
}