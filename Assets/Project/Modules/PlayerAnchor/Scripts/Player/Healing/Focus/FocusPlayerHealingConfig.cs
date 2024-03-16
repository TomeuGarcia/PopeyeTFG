using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    [System.Serializable]
    public class FocusPlayerHealingConfig
    {
        [SerializeField, Range(1, 300)] private int _healAmount = 30;
        [SerializeField, Range(1, 100)] private int _requiredFocusToHeal = 10;
        
        public int HealAmount => _healAmount;
        public int RequiredFocusToHeal => _requiredFocusToHeal;
    }
}