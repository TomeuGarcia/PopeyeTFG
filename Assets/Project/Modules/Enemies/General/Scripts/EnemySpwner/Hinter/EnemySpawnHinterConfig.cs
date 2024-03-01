using System;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Enemies.General
{
    [CreateAssetMenu(fileName = "EnemySpawnHinterConfig_NAME", 
        menuName = ScriptableObjectsHelper.ENEMYHINTS_ASSET_PATH + "EnemySpawnHinterConfig")]
    public class EnemySpawnHinterConfig : ScriptableObject
    {
        [Header("MATERIAL")] 
        [SerializeField] private Material _material;
        [SerializeField] private string _animationProperty = "_AnimationT";
        
        [Header("GROW")]
        [SerializeField] private AnimationCurve _growEase = AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private Vector3 _growEndSize = new Vector3(4,4, 4);
        [SerializeField, Range(0.01f, 5.0f)] private float _growDuration = 1.5f;
        
        [Header("DISAPPEAR")]
        [SerializeField] private AnimationCurve _disappearEase = AnimationCurve.Linear(0,0,1,1);
        [SerializeField, Range(0.0f, 1.0f)] private float _disappearWait = 0.8f; 
        [SerializeField, Range(0.01f, 5.0f)] private float _disappearDuration = 0.3f;

        [Header("USER")] 
        [SerializeField, Range(0.01f, 5.0f)] private float _userWaitDuration = 0.75f;

        public Vector3 GrowEndSize => _growEndSize;
        public AnimationCurve GrowEase => _growEase;
        public float GrowDuration => _growDuration;
        public AnimationCurve DisappearEase => _disappearEase;
        public float DisappearWait => _disappearWait;
        public float DisappearDuration => _disappearDuration;
        public Material Material => new Material(_material);
        public int AnimationPropertyId { get; private set; }
        public float UserWaitDuration => _userWaitDuration;
        
        
        private void Awake()
        {
            AnimationPropertyId = Shader.PropertyToID(_animationProperty);
        }
    }
}