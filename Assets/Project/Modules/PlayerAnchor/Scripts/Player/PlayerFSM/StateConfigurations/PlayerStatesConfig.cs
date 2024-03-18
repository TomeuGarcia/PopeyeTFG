using Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations;
using Popeye.Modules.PlayerAnchor.Player.PlayerFocus;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;


namespace Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations
{
    [CreateAssetMenu(fileName = "PlayerStatesConfig", 
        menuName = ScriptableObjectsHelper.PLAYER_ASSETS_PATH + "PlayerStatesConfig")]
    public class PlayerStatesConfig : ScriptableObject, ISpecialAttackToggleable
    {
        [Header("SPAWNING")]
        [SerializeField, Range(0.01f, 10.0f)] private float _spawnDuration = 0.5f;
        [SerializeField, Range(0.01f, 10.0f)] private float _beforeRespawnDuration = 2.0f;
        
        public float SpawnDuration => _spawnDuration;
        public float BeforeRespawnDuration => _beforeRespawnDuration;
        
        
        [Header("FALL ON VOID")]
        [SerializeField, Range(0.01f, 10.0f)] private float _fallingOnVoidDuration = 0.2f;
        [SerializeField, Range(0.01f, 10.0f)] private float _invulnerableTimeAfterVoidFallRespawn = 1.5f;

        public float FallingOnVoidDuration => _fallingOnVoidDuration;
        public float InvulnerableTimeAfterVoidFallRespawn => _invulnerableTimeAfterVoidFallRespawn;
        
        
        
        
        [Header("MOVEMENT")]
        [SerializeField, Range(0.0f, 20.0f)] private float _withoutAnchorMoveSpeed = 10.0f;
        [SerializeField, Range(0.0f, 20.0f)] private float _withAnchorMoveSpeed = 8.0f;
        [SerializeField, Range(0.0f, 20.0f)] private float _aimingMoveSpeed = 5.0f;
        [SerializeField, Range(0.0f, 20.0f)] private float _throwingAnchorMoveSpeed = 0.5f;
        [SerializeField, Range(0.0f, 20.0f)] private float _pullingAnchorMoveSpeed = 0.5f;
        [SerializeField, Range(0.0f, 20.0f)] private float _kickingAnchorMoveSpeed = 0.5f;
        [SerializeField, Range(0.0f, 20.0f)] private float _spinningAnchorMoveSpeed = 0.5f;
        [SerializeField, Range(0.0f, 20.0f)] private float _healingMoveSpeed = 2.5f;
        [SerializeField, Range(0.0f, 20.0f)] private float _fallingOnVoidMoveSpeed = 0.0f;
        [SerializeField, Range(0.0f, 20.0f)] private float _dashingMoveSpeed = 12.0f;
        [SerializeField, Range(0.0f, 20.0f)] private float _enteringSpecialAttackMoveSpeed = 0.5f;

        public float WithoutAnchorMoveSpeed => _withoutAnchorMoveSpeed + _extraSpeed;
        public float WithAnchorMoveSpeed => _withAnchorMoveSpeed + _extraSpeed;
        public float AimingMoveSpeed => _aimingMoveSpeed;
        public float ThrowingAnchorMoveSpeed => _throwingAnchorMoveSpeed;
        public float PullingAnchorMoveSpeed => _pullingAnchorMoveSpeed;
        public float KickingAnchorMoveSpeed => _kickingAnchorMoveSpeed;
        public float SpinningAnchorMoveSpeed => _spinningAnchorMoveSpeed;
        public float HealingMoveSpeed => _healingMoveSpeed;
        public float FallingOnVoidMoveSpeed => _fallingOnVoidMoveSpeed;
        public float DashingMoveSpeed => _dashingMoveSpeed;
        public float EnteringSpecialAttackMoveSpeed => _enteringSpecialAttackMoveSpeed;
        
        
        [Header("ANCHOR THROW")]
        [SerializeField, Range(0.01f, 5.0f)] private float _anchorLateThrowTime = 0.1f;
        [SerializeField, Range(0.01f, 5.0f)] private float anchorAimHeldWaitWaitTime = 0.2f;
        
        public float AnchorLateThrowTime => _anchorLateThrowTime;
        public float AnchorAimHeldWaitTime => anchorAimHeldWaitWaitTime;
        
        
        [Header("ANCHOR PICK UP")]
        [SerializeField, Range(0.0f, 20.0f)] private float _anchorPickUpDistance = 2.0f;
        
        public float AnchorPickUpDistance => _anchorPickUpDistance;
        
        
        [Header("ANCHOR KICK")]
        [SerializeField, Range(0.0f, 20.0f)] private float _anchorKickDistance = 4.0f;
        
        public float AnchorKickDistance => _anchorKickDistance;

        
        [Header("DASH")]
        [SerializeField, Range(0.01f, 10.0f)] private float _minUtilityDashDuration = 0.2f;
        [SerializeField, Range(0.01f, 10.0f)] private float _minDashDuration = 0.05f;
        [SerializeField, Range(0.01f, 10.0f)] private float _maxDashDuration = 0.35f;
        [SerializeField, Range(0.01f, 10.0f)] private float _dashInvulnerableDuration = 0.5f;
        
        public float MinUtilityDashDuration => _minUtilityDashDuration;
        public float MinDashDuration => _minDashDuration;
        public float MaxDashDuration => _maxDashDuration;
        public float DashInvulnerableDuration => _dashInvulnerableDuration;
        
        
        [Header("ROLL")]
        [SerializeField, Range(0.01f, 10.0f)] private float _minRollDuration = 0.05f;
        [SerializeField, Range(0.01f, 10.0f)] private float _maxRollDuration = 0.35f;
        [SerializeField, Range(0.01f, 10.0f)] private float _rollInvulnerableDuration = 0.5f;
        public float MinRollDuration => _minRollDuration;
        public float MaxRollDuration => _maxRollDuration;
        public float RollInvulnerableDuration => _rollInvulnerableDuration;


        [Header("TIRED")] 
        [SerializeField, Range(0.0f, 2.0f)] private float _tiredWithoutAnchorMoveSpeedMultiplier = 0.8f;
        [SerializeField, Range(0.0f, 2.0f)] private float _tiredWithAnchorMoveSpeedMultiplier = 1.0f;
        [SerializeField] private bool _dropAnchorWhenTired = false;

        public float TiredWithoutAnchorMoveSpeed => WithoutAnchorMoveSpeed * _tiredWithoutAnchorMoveSpeedMultiplier;
        public float TiredWithAnchorMoveSpeed => WithAnchorMoveSpeed * _tiredWithAnchorMoveSpeedMultiplier;
        public bool DropAnchorWhenTired => _dropAnchorWhenTired;
        
        
        [Header("HEALING")]
        [SerializeField, Range(0.01f, 10.0f)] private float _healingDuration = 0.3f; 
        public float HealingDuration => _healingDuration;
        
        
        [Header("SPECIAL ATTACK")]
        [SerializeField, Range(0.0f, 10.0f)] private float _enteringSpecialAttackDuration = 0.3f; 
        [SerializeField, Range(0.0f, 20.0f)] private float _specialAttackExtraSpeed = 5.0f; 
        public float EnteringSpecialAttackDuration => _enteringSpecialAttackDuration;
        private float _extraSpeed = 0;

        public delegate void PlayerStatesEvent();
        public PlayerStatesEvent OnSpeedValueChanged;

        public void SetDefaultMode()
        {
            _extraSpeed = 0;
            OnSpeedValueChanged?.Invoke();
        }

        public void SetSpecialAttackMode()
        {
            _extraSpeed = _specialAttackExtraSpeed;
            OnSpeedValueChanged?.Invoke();
        }
    }
}