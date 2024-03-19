using UnityEngine;

namespace Popeye.Modules.GameMenus.Generic
{
    [System.Serializable]
    public class SmartButtonAndConfig
    {
        [SerializeField] private SmartButton _smartButton;
        [SerializeField] private SmartButtonConfig _config;
            
        public SmartButton SmartButton => _smartButton;
        public SmartButtonConfig Config => _config;
    }
}