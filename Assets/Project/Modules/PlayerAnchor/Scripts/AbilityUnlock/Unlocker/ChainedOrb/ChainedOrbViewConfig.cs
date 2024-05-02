using System.Collections.Generic;
using Popeye.Modules.VFX.Generic;
using Popeye.ProjectHelpers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    [CreateAssetMenu(fileName = "ChainedOrbViewConfig", 
        menuName = ScriptableObjectsHelper.PLAYERABILITYUNLOCK_ASSETS_PATH + "ChainedOrbViewConfig")]
    public class ChainedOrbViewConfig : ScriptableObject
    {
        
        [System.Serializable]
        public class UpgradeTypeToViewData
        {
            [SerializeField] private GeneralInitializePlayerAbilityUnlockerConfig.Ability _upgradeType;
            [SerializeField] private Material _orbMaterial;
            [SerializeField] private Material _outterOrbMaterial;
            [SerializeField] private Material _breakingChainMaterial;
            [SerializeField] private Color _lightColor;
            [SerializeField] private ParticleSystem _orbHitParticlesPrefab;
            [SerializeField] private ParticleSystem _pickAbilityParticlesPrefab;

            public bool IsOfType(GeneralInitializePlayerAbilityUnlockerConfig.Ability upgradeType)
            {
                return _upgradeType == upgradeType;
            }
            public Material OrbMaterial => _orbMaterial;
            public Material OutterOrbMaterial => _outterOrbMaterial;
            public Material BreakingChainMaterial => _breakingChainMaterial;
            public Color LightColor => _lightColor;
            public ParticleSystem OrbHitParticlesPrefab => _orbHitParticlesPrefab;
            public ParticleSystem PickAbilityParticlesPrefab => _pickAbilityParticlesPrefab;
        }
        
        [System.Serializable]
        public class OrbMoveToTargetData
        {
            [SerializeField, Range(0f, 5.0f)] private float _startDelay = 0.2f;
            [SerializeField, Range(0f, 5.0f)] private float _moveDuration = 1.0f;
            [SerializeField] private AnimationCurve _moveEase = AnimationCurve.EaseInOut(0,0,1,1);
            [SerializeField] private TweenConfig _startScale;
            [SerializeField] private TweenConfig _endScale;
        
            public float StartDelay => _startDelay;
            public float MoveDuration => _moveDuration;
            public AnimationCurve MoveEase => _moveEase;
            public TweenConfig StartScale => _startScale;
            public TweenConfig EndScale => _endScale;
        }



        
        [Header("MATERIALS")]
        [SerializeField] private Material _normalChainMaterial;
        public Material NormalChainMaterial => _normalChainMaterial;

        [Header("ONHIT")]
        [SerializeField] private ParticleTypes _onHitSparklesParticleType;
        public ParticleTypes OnHitSparklesParticleType => _onHitSparklesParticleType;
        
        [Header("TYPES")]
        [SerializeField] private UpgradeTypeToViewData _defaultUpgradeTypeToViewData;
        [SerializeField] private UpgradeTypeToViewData[] _upgradeTypeToViewDatas;
        public UpgradeTypeToViewData GetViewDataFromUpgradeType(GeneralInitializePlayerAbilityUnlockerConfig.Ability upgradeType)
        {
            foreach (var upgradeTypeToViewData in _upgradeTypeToViewDatas)
            {
                if (upgradeTypeToViewData.IsOfType(upgradeType))
                {
                    return upgradeTypeToViewData;
                }
            }

            return _defaultUpgradeTypeToViewData;
        }
        
        
        
        [Header("CHAINS DISAPPEAR")]
        [SerializeField] private Vector2 _breakStepDuration = new Vector2(0.1f, 0.2f);
        [SerializeField] private Vector3 _breakPunch = Vector3.one * 0.5f;
        
        public Vector2 BreakStepDuration => _breakStepDuration;
        public Vector3 BreakPunch => _breakPunch;
        
        
        
        [Header("ORB HIT")]
        [SerializeField] private float _orbHitMovePunch = 2.0f;
        [SerializeField] private float _orbHitRotateAmount = 180.0f;
        [SerializeField] private float _orbHitDuration = 1.0f;
        [SerializeField] private float _orbChainsStartBreakingDelay = 0.3f;
        [SerializeField] private float _orbChainsDisappearDelay = 0.1f;
        
        public float OrbHitMovePunch => _orbHitMovePunch;
        public float OrbHitRotateAmount => _orbHitRotateAmount;
        public float OrbHitDuration => _orbHitDuration;
        public float OrbChainsStartBreakingDelay => _orbChainsStartBreakingDelay;
        public float OrbChainsDisappearDelay => _orbChainsDisappearDelay;
        
        
        [Header("ORBITAL CHAINS")]
        [SerializeField] private float _sparkForwardCoef;
        [SerializeField] private float _sparkUpwardsCoef;
        public float SparkForwardCoef => _sparkForwardCoef;
        public float SparkUpwardsCoef => _sparkUpwardsCoef;
        
        [Header("ORBITAL CHAINS")]
        [SerializeField] private List<float> _orbitalChainRotationSpeeds = new();
        public List<float> OrbitalChainRotationSpeeds => _orbitalChainRotationSpeeds;
        
        [Header("ORB MOVE")]
        [SerializeField] private OrbMoveToTargetData _orbMoveToTarget;
        public OrbMoveToTargetData OrbMoveToTarget => _orbMoveToTarget;

    }
}