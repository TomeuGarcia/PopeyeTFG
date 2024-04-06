using System;
using Popeye.ProjectHelpers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    [CreateAssetMenu(fileName = "AbilityUnlockerChristalViewConfig", 
        menuName = ScriptableObjectsHelper.PLAYERABILITYUNLOCK_ASSETS_PATH + "ChristalViewConfig")]
    public class AbilityUnlockerChristalViewConfig : ScriptableObject
    {
        
        [Header("MATERIALS")] 
        [SerializeField] private Material _explodingChainSharedMaterial;
        [SerializeField] private string _explodeStartTimeProperty = "_ExplodeStartTime";
        public int ExplodeStartTimePropertyId { get; private set; }

        public Material ExplodingChainSharedMaterial => _explodingChainSharedMaterial;
        
        
        
        [Header("0. IDLE")]
        [SerializeField, Range(0.0f, 10.0f)] private float _idleDelay;
        [SerializeField] private TweenPunchConfig _idleChainPunch;
        public float IdleDelay => _idleDelay;
        public TweenPunchConfig IdleChainPunch => _idleChainPunch;        
        
        
        
        [Header("1. UNLOCK ANIMATION")]
        [SerializeField, Range(0.0f, 10.0f)] private float _unlockDelay = 0.1f;
        [SerializeField] private TweenPunchConfig _unlockScalePunch;
        [SerializeField] private TweenPunchConfig _unlockRotationPunch;
        public float UnlockDelay => _unlockDelay;
        public TweenPunchConfig UnlockScalePunch => _unlockScalePunch;
        public TweenPunchConfig UnlockRotationPunch => _unlockRotationPunch;


        [Header("2. EXPLODE")]
        [SerializeField, Range(0f, 5.0f)] private float _explodeDelay = 0.2f;
        public float ExplodeDelay => _explodeDelay;
        
        
        [Header("3. CORE MOVE")]
        [SerializeField, Range(0f, 5.0f)] private float _coreMoveDelay = 0.2f;
        [SerializeField, Range(0f, 5.0f)] private float _coreMoveDuration = 1.0f;
        [SerializeField] private AnimationCurve _coreMoveEase = AnimationCurve.EaseInOut(0,0,1,1);
        [SerializeField] private TweenPunchConfig _coreScalePunch;
        public float CoreMoveDelay => _coreMoveDelay;
        public float CoreMoveDuration => _coreMoveDuration;
        public AnimationCurve CoreMoveEase => _coreMoveEase;
        public TweenPunchConfig CoreScalePunch => _coreScalePunch;


        
        private void OnValidate()
        {
            ExplodeStartTimePropertyId = Shader.PropertyToID(_explodeStartTimeProperty);
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}