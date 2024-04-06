using DG.Tweening;
using Popeye.Modules.VFX.Generic;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    [CreateAssetMenu(fileName = "PlayerParticlesViewConfig", 
        menuName = ScriptableObjectsHelper.PLAYER_ASSETS_PATH + "PlayerParticlesViewConfig")]
    public class PlayerParticlesViewConfig : ScriptableObject
    {
        [Header("HEAL")]
        [SerializeField] private ParticleTypes _healProcessParticleType;
        [SerializeField] private ParticleTypes _healCompletedParticleType;
        [SerializeField] private ParticleTypes _enragedStartParticleTypes;
        [SerializeField] private ParticleTypes _enragedParticleTypes;
        
        [Header("DASH")]
        [SerializeField] private ParticleTypes _dashTrailParticleType;
        [SerializeField] private ParticleTypes _dashDisappearParticleType;
        [SerializeField] private ParticleTypes _dashAppearParticleType;
        [SerializeField] private ParticleTypes _dashGhostParticleType;

        [SerializeField] private float _trailSpawnDelay = 0.05f;
        [SerializeField] private float _trailRecycleDelay = 0.05f;
        [SerializeField] private Vector3 _dashTrailRotation;

        [SerializeField] private Ease _dashTrailRotationEase;
        [SerializeField] private Ease _dashTrailScaleEase;
        
        public ParticleTypes HealProcessParticleType => _healProcessParticleType;
        public ParticleTypes HealCompletedParticleType => _healCompletedParticleType;
        public ParticleTypes EnragedStartParticleTypes => _enragedStartParticleTypes;
        public ParticleTypes EnragedParticleTypes => _enragedParticleTypes;
        public ParticleTypes DashTrailParticleType => _dashTrailParticleType;
        public ParticleTypes DashDisappearParticleType => _dashDisappearParticleType;
        public ParticleTypes DashAppearParticleType => _dashAppearParticleType;
        public ParticleTypes DashGhostParticleType => _dashGhostParticleType;
        public float TrailSpawnDelay => _trailSpawnDelay;
        public float TrailRecycleDelay => _trailRecycleDelay;
        public Vector3 DashTrailRotation => _dashTrailRotation;
        public Ease DashTrailRotationEase => _dashTrailRotationEase;
        public Ease DashTrailScaleEase => _dashTrailScaleEase;
    }
}