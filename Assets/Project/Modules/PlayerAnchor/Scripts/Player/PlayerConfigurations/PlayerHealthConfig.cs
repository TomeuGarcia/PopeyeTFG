using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations
{
    [System.Serializable]
    public class PlayerHealthConfig
    {
        [System.Serializable]
        public class HealthConfigData
        {
            [SerializeField, Range(1, 20)] private int _startingMaxHealth = 4;
            [SerializeField, Range(1, 20)] private int _startingHealth = 4;
            [SerializeField, Range(1, 20)] private int _healthIncreaseAmount = 1;
            
            public int StartingMaxHealth => _startingMaxHealth;
            public int StartingHealth => _startingHealth;
            public int HealthIncreaseAmount => _healthIncreaseAmount;
        }
        
        
        [SerializeField] private HealthConfigData _healthData;
        [SerializeField, Range(0.01f, 5.0f)] private float _invulnerableDurationAfterTakingDamage = 1.0f;
        
        
        public HealthConfigData HealthData => _healthData;

        public float InvulnerableDurationAfterTakingDamage => _invulnerableDurationAfterTakingDamage;
    }
}