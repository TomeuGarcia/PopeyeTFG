using Popeye.Modules.CombatSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "AnchorDamageConfig", 
        menuName = ScriptableObjectsHelper.ANCHOR_ASSETS_PATH + "AnchorDamageConfig")]
    public class AnchorDamageConfig : ScriptableObject
    {
        [Header("THROW")]
        [SerializeField] private DamageHitConfig _anchorThrowDamageHit;
        [SerializeField, Range(0f, 5.0f)] private float _throwDamageExtraDuration = 0.2f;
        
        public DamageHitConfig AnchorThrowDamageHit => _anchorThrowDamageHit;
        public float ThrowDamageExtraDuration => _throwDamageExtraDuration;

        
        [Header("PULL")]
        [SerializeField] private DamageHitConfig _anchorPullDamageHit;
        [SerializeField, Range(0f, 5.0f)] private float _pullDamageExtraDuration = 0.2f;
        [SerializeField, Range(0f, 20.0f)] private float _pullKnockbackDistanceFromPlayer = 7.0f;
        
        public DamageHitConfig AnchorPullDamageHit => _anchorPullDamageHit;
        public float PullDamageExtraDuration => _pullDamageExtraDuration;
        public float PullKnockbackDistanceFromPlayer => _pullKnockbackDistanceFromPlayer;
        
        
        [Header("KICK")]
        [SerializeField] private DamageHitConfig _anchorKickDamageHit;
        [SerializeField, Range(0f, 5.0f)] private float _kickDamageExtraDuration = 0.2f;
        
        public DamageHitConfig AnchorKickDamageHit => _anchorKickDamageHit;
        public float KickDamageExtraDuration => _kickDamageExtraDuration;
        
        
        [Header("VERTICAL LAND")]
        [SerializeField] private DamageHitConfig _anchorVerticalLandDamageHit;
        [SerializeField, Range(0f, 5.0f)] private float _verticalLandDamageExtraDuration = 0.2f;
        
        public DamageHitConfig AnchorVerticalLandDamageHit => _anchorVerticalLandDamageHit;
        public float VerticalLandDamageExtraDuration => _verticalLandDamageExtraDuration;
        
        
        [Header("SPIN")]
        [SerializeField] private DamageHitConfig _anchorSpinDamageHit;
        public DamageHitConfig AnchorSpinDamageHit => _anchorSpinDamageHit;



    }
}