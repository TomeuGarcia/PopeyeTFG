using FMODUnity;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    [CreateAssetMenu(fileName = "OneShotSound_NAME", 
        menuName = ScriptableObjectsHelper.SOUNDSYSTEM_ASSETS_PATH + "OneShotSound")]
    public class OneShotFMODSound : ScriptableObject
    {
        [SerializeField] private EventReference _eventReference;
        [SerializeField] private SoundParameter[] _parameters;
        
        public EventReference EventReference => _eventReference;
        public SoundParameter[] Parameters => _parameters;
    }
}