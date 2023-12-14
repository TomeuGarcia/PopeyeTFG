using System;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using Popeye.Modules.ValueStatSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations
{
    
    [CreateAssetMenu(fileName = "PlayerGeneralConfig", 
        menuName = PlayerConfigHelper.SO_ASSETS_PATH + "PlayerGeneralConfig")]
    public class PlayerGeneralConfig : ScriptableObject
    {
        [Header("Health")] 
        [SerializeField, Range(0, 300)] private int _maxHealth = 100;
        [SerializeField, Range(0, 300)] private int _potionHealAmount = 30;
        [SerializeField, Range(0.0f, 5.0f)] private float _invulnerableDurationAfterHit = 1.0f;
        
        
        public int MaxHealth => _maxHealth;
        public int PotionHealAmount => _potionHealAmount;
        public float InvulnerableDurationAfterHit => _invulnerableDurationAfterHit;


        [Header("OTHER CONFIGURATIONS")] 
        [SerializeField] private PlayerMovesetConfig _playerMovesetConfig;
        [SerializeField] private PlayerStatesConfig _playerStatesConfig;
        [SerializeField] private TimeStaminaConfig_SO _playerStaminaConfig;

        public PlayerMovesetConfig MovesetConfig => _playerMovesetConfig;
        public PlayerStatesConfig StatesConfig => _playerStatesConfig;
        public TimeStaminaConfig_SO StaminaConfig => _playerStaminaConfig;
    }
}