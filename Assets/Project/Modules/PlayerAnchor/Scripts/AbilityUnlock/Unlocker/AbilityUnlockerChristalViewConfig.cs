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
        [Header("IDLE")]
        [SerializeField] private float _idleDelay;
        [SerializeField] private TweenPunchConfig _idleChainPunch;
        public float IdleDelay => _idleDelay;
        public TweenPunchConfig IdleChainPunch => _idleChainPunch;        
        
        
        
        [Header("UNLOCK ANIMATION")]
        [SerializeField] private TweenPunchConfig _unlockScalePunch;
        [SerializeField] private TweenPunchConfig _unlockRotationPunch;
        public TweenPunchConfig UnlockScalePunch => _unlockScalePunch;
        public TweenPunchConfig UnlockRotationPunch => _unlockRotationPunch;


        [Header("EXPLODE")]
        [SerializeField, Range(0f, 5.0f)] private float _explodeDelay = 0.2f;
        public float ExplodeDelay => _explodeDelay;
        

        [Header("MATERIALS")] 
        [SerializeField] private Material _explodingChainSharedMaterial;
        [SerializeField] private string _explodeStartTimeProperty = "_ExplodeStartTime";
        public int ExplodeStartTimePropertyId { get; private set; }

        public Material ExplodingChainSharedMaterial => _explodingChainSharedMaterial;


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