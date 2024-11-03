using NaughtyAttributes;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.PlayerAnchor.Player.PlayerFocus;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "AnchorDamageConfig", 
        menuName = ScriptableObjectsHelper.ANCHOR_ASSETS_PATH + "AnchorDamageConfig")]
    public class AnchorDamageConfig : ScriptableObject
    {
        [Header("THROW")]
        [Expandable] [SerializeField] private DamageHitConfig _throwDamageHit;
        [SerializeField, Range(0f, 5.0f)] private float _throwDamageExtraDuration = 0.2f;
        
        public float ThrowDamageExtraDuration => _throwDamageExtraDuration;

        
        [Header("PULL")]
        [Expandable] [SerializeField] private DamageHitConfig _pullDamageHit;
        [SerializeField, Range(0f, 5.0f)] private float _pullDamageExtraDuration = 0.2f;
        [SerializeField, Range(0f, 20.0f)] private float _pullKnockbackDistanceFromPlayer = 7.0f;
        
        public float PullDamageExtraDuration => _pullDamageExtraDuration;
        public float PullKnockbackDistanceFromPlayer => _pullKnockbackDistanceFromPlayer;


        [Header("VERTICAL LAND")]
        [Expandable] [SerializeField] private DamageHitConfig _verticalLandDamageHit;
        [SerializeField, Range(0f, 5.0f)] private float _verticalLandDamageDuration = 0.1f;
        
        public float VerticalLandDamageDuration => _verticalLandDamageDuration;
        
        
        [Header("SPIN")]
        [Expandable] [SerializeField] private DamageHitConfig _spinDamageHit;

        
        public DamageHit ThrowDamageHit => _defaultHitsGroup.ThrowDamageHit;
        public DamageHit PullDamageHit => _defaultHitsGroup.PullDamageHit;
        public DamageHit SpinDamageHit => _defaultHitsGroup.SpinDamageHit;
        public DamageHit VerticalLandDamageHit => _defaultHitsGroup.VerticalLandDamageHit;

        
        private AnchorDamageHitsGroup _defaultHitsGroup;

        private class AnchorDamageHitsGroup
        {
            public DamageHit ThrowDamageHit;
            public DamageHit PullDamageHit;
            public DamageHit SpinDamageHit;
            public DamageHit VerticalLandDamageHit;

            public AnchorDamageHitsGroup(
                DamageHitConfig throwConfig,
                DamageHitConfig pullConfig,
                DamageHitConfig spinConfig,
                DamageHitConfig verticalLandConfig)
            {
                ThrowDamageHit = new DamageHit(throwConfig); 
                PullDamageHit = new DamageHit(pullConfig); 
                VerticalLandDamageHit = new DamageHit(verticalLandConfig); 
                SpinDamageHit = new DamageHit(spinConfig); 
            }
        }


        public void Init()
        {
            _defaultHitsGroup = new AnchorDamageHitsGroup(
                _throwDamageHit,
                _pullDamageHit,
                _spinDamageHit,
                _verticalLandDamageHit
            );
        }

    }
}