using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    [System.Serializable]
    public class PlayerFocusHealingConfig
    {
        [SerializeField, Range(1, 100)] private int _requiredFocusToPerform = 10;
        [SerializeField, Range(1, 300)] private int _healAmount = 30;
        
        public int HealAmount => _healAmount;
        public int RequiredFocusToPerform => _requiredFocusToPerform;
    }
}