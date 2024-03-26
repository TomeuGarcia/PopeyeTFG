using NaughtyAttributes;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.PlayerAnchor.Player.PlayerFocus;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "AnchorDamageConfig", 
        menuName = ScriptableObjectsHelper.ANCHOR_ASSETS_PATH + "AnchorDamageConfig")]
    public class AnchorDamageConfig : ScriptableObject, ISpecialAttackToggleable
    {
        [Header("THROW")]
        [Expandable] [SerializeField] private DamageHitConfig _throwDamageHit;
        [Expandable] [SerializeField] private DamageHitConfig _throwDamageHit_Enraged;
        [SerializeField, Range(0f, 5.0f)] private float _throwDamageExtraDuration = 0.2f;
        
        public float ThrowDamageExtraDuration => _throwDamageExtraDuration;

        
        [Header("PULL")]
        [Expandable] [SerializeField] private DamageHitConfig _pullDamageHit;
        [Expandable] [SerializeField] private DamageHitConfig _pullDamageHit_Enraged;
        [SerializeField, Range(0f, 5.0f)] private float _pullDamageExtraDuration = 0.2f;
        [SerializeField, Range(0f, 20.0f)] private float _pullKnockbackDistanceFromPlayer = 7.0f;
        
        public float PullDamageExtraDuration => _pullDamageExtraDuration;
        public float PullKnockbackDistanceFromPlayer => _pullKnockbackDistanceFromPlayer;
        
        
        [Header("KICK")]
        [Expandable] [SerializeField] private DamageHitConfig _kickDamageHit;
        [Expandable] [SerializeField] private DamageHitConfig _kickDamageHit_Enraged;
        [SerializeField, Range(0f, 5.0f)] private float _kickDamageExtraDuration = 0.2f;
        
        public float KickDamageExtraDuration => _kickDamageExtraDuration;
        
        
        [Header("VERTICAL LAND")]
        [Expandable] [SerializeField] private DamageHitConfig _verticalLandDamageHit;
        [Expandable] [SerializeField] private DamageHitConfig _verticalLandDamageHit_Enraged;
        [SerializeField, Range(0f, 5.0f)] private float _verticalLandDamageExtraDuration = 0.2f;
        
        public float VerticalLandDamageExtraDuration => _verticalLandDamageExtraDuration;
        
        
        [Header("SPIN")]
        [Expandable] [SerializeField] private DamageHitConfig _spinDamageHit;
        [Expandable] [SerializeField] private DamageHitConfig _spinDamageHit_Enraged;

        

        private bool IsEnraged => false;
        
        public DamageHit ThrowDamageHit => _currentHitsGroup.ThrowDamageHit;
        public DamageHit PullDamageHit => _currentHitsGroup.PullDamageHit;
        public DamageHit KickDamageHit => _currentHitsGroup.KickDamageHit;
        public DamageHit SpinDamageHit => _currentHitsGroup.SpinDamageHit;
        public DamageHit VerticalLandDamageHit => _currentHitsGroup.VerticalLandDamageHit;

        
        private AnchorDamageHitsGroup _defaultHitsGroup;
        private AnchorDamageHitsGroup _enragedHitsGroup;
        private AnchorDamageHitsGroup _currentHitsGroup;
        
        private class AnchorDamageHitsGroup
        {
            public DamageHit ThrowDamageHit;
            public DamageHit PullDamageHit;
            public DamageHit KickDamageHit;
            public DamageHit SpinDamageHit;
            public DamageHit VerticalLandDamageHit;

            public AnchorDamageHitsGroup(
                DamageHitConfig throwConfig,
                DamageHitConfig pullConfig,
                DamageHitConfig kickConfig,
                DamageHitConfig spinConfig,
                DamageHitConfig verticalLandConfig)
            {
                ThrowDamageHit = new DamageHit(throwConfig); 
                PullDamageHit = new DamageHit(pullConfig); 
                KickDamageHit = new DamageHit(kickConfig); 
                VerticalLandDamageHit = new DamageHit(verticalLandConfig); 
                SpinDamageHit = new DamageHit(spinConfig); 
            }
        }
        
        
        public void Init()
        {
            _defaultHitsGroup = new AnchorDamageHitsGroup(
                _throwDamageHit,
                _pullDamageHit, 
                _kickDamageHit, 
                _spinDamageHit, 
                _verticalLandDamageHit
            );
            
            _enragedHitsGroup = new AnchorDamageHitsGroup(
                _throwDamageHit_Enraged,
                _pullDamageHit_Enraged, 
                _kickDamageHit_Enraged, 
                _spinDamageHit_Enraged, 
                _verticalLandDamageHit_Enraged
            );

            SetDefaultMode();
        }
        
        public void SetDefaultMode()
        {
            _currentHitsGroup = _defaultHitsGroup;
        }

        public void SetSpecialAttackMode()
        {
            _currentHitsGroup = _enragedHitsGroup;
        }
    }
}