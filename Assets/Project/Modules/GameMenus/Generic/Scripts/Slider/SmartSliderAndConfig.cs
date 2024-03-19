using UnityEngine;

namespace Popeye.Modules.GameMenus.Generic
{
    [System.Serializable]
    public class SmartSliderAndConfig
    {
        [SerializeField] private SmartSlider _smartSlider;
        [SerializeField] private SmartSliderConfig _config;
            
        public SmartSlider SmartSlider => _smartSlider;
        public SmartSliderConfig Config => _config;
    }
}