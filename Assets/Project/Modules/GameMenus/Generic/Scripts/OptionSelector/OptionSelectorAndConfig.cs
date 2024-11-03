using UnityEngine;

namespace Popeye.Modules.GameMenus.Generic
{
    [System.Serializable]
    public class OptionSelectorAndConfig
    {
        [SerializeField] private OptionSelector _optionSelector;
        [SerializeField] private OptionSelectorConfig _config;
            
        public OptionSelector OptionSelector => _optionSelector;
        public OptionSelectorConfig Config => _config;
    }
}