using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations
{
    [System.Serializable]
    public class PlayerHealthConfig
    {
        [SerializeField, Range(1, 300)] private int _maxHealth = 100;
        [SerializeField, Range(0.01f, 5.0f)] private float _invulnerableDurationAfterTakingDamage = 1.0f;
        
        
        public int MaxHealth => _maxHealth;

        public float InvulnerableDurationAfterTakingDamage => _invulnerableDurationAfterTakingDamage;
    }
}