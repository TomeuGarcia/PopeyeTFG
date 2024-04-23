using Popeye.ProjectHelpers;
using UnityEngine;
using NaughtyAttributes;

namespace Popeye.Modules.VFX.Generic.MaterialInterpolationConfiguration
{
    [CreateAssetMenu(fileName = "MF_SetupConfig_NAME", 
        menuName = ScriptableObjectsHelper.VFX_ASSETS_PATH + "MF_SetupConfig", order = 0)]

    public class MaterialFloatSetupConfig : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private float _initialValue;
            
        private int _property;

        public int ID => _property;
        public string Name => _name;
        public float InitialValue => _initialValue;

        private void Awake()
        {
            _property = Shader.PropertyToID(_name);
        }
    }
}