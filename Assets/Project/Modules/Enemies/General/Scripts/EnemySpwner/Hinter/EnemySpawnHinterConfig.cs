using System;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Enemies.General
{
    [CreateAssetMenu(fileName = "EnemySpawnHinterConfig", 
        menuName = ScriptableObjectsHelper.ENEMIES_ASSET_PATH + "EnemySpawnHinterConfig")]
    public class EnemySpawnHinterConfig : ScriptableObject
    {
        [Header("MATERIAL")] 
        [SerializeField] private string _animationProperty = "_AnimationT";
        
        [Header("GROW")]
        [SerializeField] private AnimationCurve _growEase = AnimationCurve.Linear(0,0,1,1);
        
        [Header("DISAPPEAR")]
        [SerializeField] private AnimationCurve _disappearEase = AnimationCurve.Linear(0,0,1,1);
        [SerializeField, Range(0.0f, 1.0f)] private float _disappearWait = 0.8f; 
        [SerializeField, Range(0.01f, 5.0f)] private float _disappearDuration = 0.3f;

        public AnimationCurve GrowEase => _growEase;
        public AnimationCurve DisappearEase => _disappearEase;
        public float DisappearWait => _disappearWait;
        public float DisappearDuration => _disappearDuration;
        public int AnimationPropertyId { get; private set; }
        
        
        private void Awake()
        {
            AnimationPropertyId = Shader.PropertyToID(_animationProperty);
        }
    }
}