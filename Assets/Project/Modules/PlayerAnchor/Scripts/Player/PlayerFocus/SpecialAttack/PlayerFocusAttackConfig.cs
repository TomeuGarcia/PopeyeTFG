using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    [System.Serializable]
    public class PlayerFocusAttackConfig
    {
        [SerializeField, Range(1, 100)] private int _requiredFocusToPerform = 100;
        [SerializeField, Range(0.01f, 10.0f)] private float _attackDuration = 5.0f;
        
        public int RequiredFocusToPerform => _requiredFocusToPerform;
        public float AttackDuration => _attackDuration;
    }
}