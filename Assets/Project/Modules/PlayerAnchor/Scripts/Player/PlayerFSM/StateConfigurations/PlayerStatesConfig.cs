using Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations;
using UnityEngine;


namespace Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations
{
    [CreateAssetMenu(fileName = "PlayerStatesConfig", 
        menuName = PlayerConfigHelper.SO_ASSETS_PATH + "PlayerStatesConfig")]
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
        [SerializeField, Range(0.0f, 20.0f)] private float _tiredMoveSpeed = 2.5f;
        [SerializeField, Range(0.0f, 20.0f)] private float _healingMoveSpeed = 2.5f;

        public float WithoutAnchorMoveSpeed => _withoutAnchorMoveSpeed;
        public float WithAnchorMoveSpeed => _withAnchorMoveSpeed;
        public float AimingMoveSpeed => _aimingMoveSpeed;
        public float ThrowingAnchorMoveSpeed => _throwingAnchorMoveSpeed;
        public float PullingAnchorMoveSpeed => _pullingAnchorMoveSpeed;
        public float KickingAnchorMoveSpeed => _kickingAnchorMoveSpeed;
        public float TiredMoveSpeed => _tiredMoveSpeed;
        public float HealingMoveSpeed => _healingMoveSpeed;
        
        
        [Header("ANCHOR PICK UP")]
        [SerializeField, Range(0.0f, 20.0f)] private float _anchorPickUpDistance = 2.0f;
        
        public float AnchorPickUpDistance => _anchorPickUpDistance;
        
        
        [Header("ANCHOR KICK")]
        [SerializeField, Range(0.0f, 20.0f)] private float _anchorKickDistance = 4.0f;
        
        public float AnchorKickDistance => _anchorKickDistance;

        
        [Header("DASH")]
        [SerializeField, Range(0.01f, 10.0f)] private float _dashDuration = 0.25f;
        [SerializeField, Range(0.01f, 10.0f)] private float _dashInvulnerableDuration = 0.5f;
        
        public float DashDuration => _dashDuration;
        public float DashInvulnerableDuration => _dashInvulnerableDuration;

    }
}