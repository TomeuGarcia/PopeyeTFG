using FMODUnity;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    [CreateAssetMenu(fileName = "OneShotFMODSoundAsset_NAME", 
        menuName = ScriptableObjectsHelper.SOUNDSYSTEM_ASSETS_PATH + "OneShotFMODSoundAsset")]
    public class OneShotFMODSoundAsset : ScriptableObject, IOneShotFMODSound
    {
        [SerializeField] private EventReference _eventReference;
        [SerializeField] private SoundParameter[] _parameters;
        
        public EventReference EventReference => _eventReference;
        public SoundParameter[] Parameters => _parameters;
    }
}