using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    [CreateAssetMenu(fileName = "GlobalParametersConfig", 
        menuName = ScriptableObjectsHelper.SOUNDSYSTEM_ASSETS_PATH + "GlobalParametersConfig")]
    public class GlobalParametersConfig : ScriptableObject
    {
        [SerializeField] private SoundParameter[] _parameters;

        public SoundParameter[] Parameters => _parameters;
    }
}