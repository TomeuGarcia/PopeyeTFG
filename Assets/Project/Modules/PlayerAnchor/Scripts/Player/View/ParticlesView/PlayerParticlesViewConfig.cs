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
        [Header("DASH")]
        [SerializeField] private ParticleTypes _dashTrailParticleType;
        [SerializeField] private ParticleTypes _dashDisappearParticleType;
        [SerializeField] private ParticleTypes _dashAppearParticleType;

        [SerializeField] private float _trailSpawnDelay = 0.05f;
        [SerializeField] private Vector3 _dashTrailRotation;

        [SerializeField] private Ease _dashTrailRotationEase;
        [SerializeField] private Ease _dashTrailScaleEase;
        
        public ParticleTypes DashTrailParticleType => _dashTrailParticleType;
        public ParticleTypes DashDisappearParticleType => _dashDisappearParticleType;
        public ParticleTypes DashAppearParticleType => _dashAppearParticleType;
        public float TrailSpawnDelay => _trailSpawnDelay;
        public Vector3 DashTrailRotation => _dashTrailRotation;
        public Ease DashTrailRotationEase => _dashTrailRotationEase;
        public Ease DashTrailScaleEase => _dashTrailScaleEase;
    }
}