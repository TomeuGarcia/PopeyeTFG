using Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;


namespace Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations
{
    [CreateAssetMenu(fileName = "PlayerStatesConfig", 
        menuName = ScriptableObjectsHelper.PLAYER_ASSETS_PATH + "PlayerStatesConfig")]
    public class PlayerStatesConfig : ScriptableObject
    {
        [Header("SPAWNING")]
        [SerializeField, Range(0.01f, 10.0f)] private float _spawnDuration = 0.5f;
        [SerializeField, Range(0.01f, 10.0f)] private float _beforeRespawnDuration = 2.0f;
        
        public float SpawnDuration => _spawnDuration;
        public float BeforeRespawnDuration => _beforeRespawnDuration;
        
        
        [Header("MOVEMENT")]
        [SerializeField, Range(0.0f, 20.0f)]private float _withoutAnchorMoveSpeed = 10.0f;
        [SerializeField, Range(0.0f, 20.0f)] private float _withAnchorMoveSpeed = 8.0f;
        [SerializeField, Range(0.0f, 20.0f)] private float _aimingMoveSpeed = 5.0f;
        [SerializeField, Range(0.0f, 20.0f)] private float _throwingAnchorMoveSpeed = 0.5f;
        [SerializeField, Range(0.0f, 20.0f)] private float _pullingAnchorMoveSpeed = 0.5f;
        [SerializeField, Range(0.0f, 20.0f)] private float _kickingAnchorMoveSpeed = 0.5f;
        [SerializeField, Range(0.0f, 20.0f)] private float _spinningAnchorMoveSpeed = 0.5f;
        [SerializeField, Range(0.0f, 20.0f)] private float _tiredMoveSpeed = 2.5f;
        [SerializeField, Range(0.0f, 20.0f)] private float _healingMoveSpeed = 2.5f;

        public float WithoutAnchorMoveSpeed => _withoutAnchorMoveSpeed;
        public float WithAnchorMoveSpeed => _withAnchorMoveSpeed;
        public float AimingMoveSpeed => _aimingMoveSpeed;
        public float ThrowingAnchorMoveSpeed => _throwingAnchorMoveSpeed;
        public float PullingAnchorMoveSpeed => _pullingAnchorMoveSpeed;
        public float KickingAnchorMoveSpeed => _kickingAnchorMoveSpeed;
        public float SpinningAnchorMoveSpeed => _spinningAnchorMoveSpeed;
        public float TiredMoveSpeed => _tiredMoveSpeed;
        public float HealingMoveSpeed => _healingMoveSpeed;
        
        
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
        [SerializeField, Range(0.01f, 10.0f)] private float _minDashDuration = 0.05f;
        [SerializeField, Range(0.01f, 10.0f)] private float _maxDashDuration = 0.35f;
        [SerializeField, Range(0.01f, 10.0f)] private float _dashInvulnerableDuration = 0.5f;
        
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

    }
}