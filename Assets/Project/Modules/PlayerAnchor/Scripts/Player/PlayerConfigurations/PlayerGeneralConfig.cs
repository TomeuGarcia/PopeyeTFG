using System;
using NaughtyAttributes;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using Popeye.Modules.PlayerAnchor.Player.Stamina;
using Popeye.Modules.ValueStatSystem;
using Popeye.ProjectHelpers;
using Popeye.Scripts.Collisions;
using Popeye.Scripts.ObjectTypes;
using UnityEngine;
using UnityEngine.Serialization;


namespace Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations
{
    
    [CreateAssetMenu(fileName = "PlayerGeneralConfig", 
        menuName = ScriptableObjectsHelper.PLAYER_ASSETS_PATH + "PlayerGeneralConfig")]
    public class PlayerGeneralConfig : ScriptableObject
    {
        [Header("OTHER CONFIGURATIONS")] 
        [Expandable] [SerializeField] private PlayerMovesetConfig _playerMovesetConfig;
        [Expandable] [SerializeField] private PlayerStatesConfig _playerStatesConfig;
        public PlayerMovesetConfig MovesetConfig => _playerMovesetConfig;
        public PlayerStatesConfig StatesConfig => _playerStatesConfig;
        
        
        [Header("STAMINA")]
        [Expandable] [SerializeField] private PlayerStaminaSystemConfig _playerStaminaConfig;
        public PlayerStaminaSystemConfig StaminaConfig => _playerStaminaConfig;

        
        [Header("HEALTH")]
        [SerializeField] private PlayerHealthConfig _playerHealthConfig;
        [SerializeField] private PlayerHealingConfig _healingConfig;
        public PlayerHealthConfig PlayerHealthConfig => _playerHealthConfig;
        public PlayerHealingConfig HealingConfig => _healingConfig;

        
        [Header("GROUND / VOID checking")] 
        [SerializeField] private CollisionProbingConfig _safeGroundProbingConfig;
        [SerializeField] private CollisionProbingConfig _onVoidProbingConfig;
        [SerializeField] private DamageHitConfig _voidFallDamageConfig;
        [SerializeField] private ObjectTypeAsset _notSafeGroundType;
        [SerializeField] private Vector3 _respawnFromVoidPositionOffset = new Vector3(0,2,0);
        
        public CollisionProbingConfig SafeGroundProbingConfig => _safeGroundProbingConfig;
        public CollisionProbingConfig OnVoidProbingConfig => _onVoidProbingConfig;
        public DamageHitConfig VoidFallDamageConfig => _voidFallDamageConfig;
        public ObjectTypeAsset NotSafeGroundType => _notSafeGroundType;
        public Vector3 RespawnFromVoidPositionOffset => _respawnFromVoidPositionOffset;

        
        [Header("VIEW")] 
        [SerializeField] private PlayerGeneralViewConfig _generalViewConfig;
        
        public PlayerGeneralViewConfig GeneralViewConfig => _generalViewConfig;
    }
}