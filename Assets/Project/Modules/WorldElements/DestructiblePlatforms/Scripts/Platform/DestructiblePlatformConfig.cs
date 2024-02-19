using System;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Project.Modules.WorldElements.DestructiblePlatforms
{
    [CreateAssetMenu(fileName = "DestructiblePlatformConfig", 
        menuName = ScriptableObjectsHelper.WORLDELEMENTS_ASSETS_PATH + "DestructiblePlatformConfig")]
    public class DestructiblePlatformConfig : ScriptableObject
    {
        [SerializeField, Range(0.01f, 1.0f)] private float _breakOverTimeStartDelay = 0.25f;
        [SerializeField, Range(0.01f, 20.0f)] private float _breakOverTimeDuration = 2.0f;
        [SerializeField, Range(0.01f, 1.0f)] private float _enterBrokenStateDelay = 0.15f;
        [SerializeField, Range(0.01f, 20.0f)] private float _brokenStateStateDuration = 3.0f;

        
        public float BreakOverTimeStartDelay => _breakOverTimeStartDelay;
        public float BreakOverTimeDuration => _breakOverTimeDuration;
        public float EnterBrokenStateDelay => _enterBrokenStateDelay;
        public float BrokenStateDuration => _brokenStateStateDuration;


        [System.Serializable]
        public class AnimationConfigData
        {
            [SerializeField] private Material _sharedMaterial;
            
            [SerializeField] private Color _breakingColor = Color.red;
            [SerializeField] private Color _regeneratingColor = Color.cyan;

            [SerializeField, Range(0.01f, 2.0f)] private float _breakingDuration = 1.0f;
            [SerializeField, Range(0.01f, 2.0f)] private float _regeneratingDuration = 1.0f;

            [SerializeField] private string _colorProperty = "_FadingColor";
            [SerializeField] private string _animationProperty = "_AnimationT";


            public Material SharedMaterial => _sharedMaterial;
            public Color BreakingColor => _breakingColor;
            public Color RegeneratingColor => _regeneratingColor;
            public float BreakingDuration => _breakingDuration;
            public float RegeneratingDuration => _regeneratingDuration;
            public int ColorPropertyID { get; private set; }
            public int AnimationPropertyID { get; private set; }

            public void OnValidate()
            {
                ColorPropertyID = Shader.PropertyToID(_colorProperty);
                AnimationPropertyID = Shader.PropertyToID(_animationProperty);
            }
        }

        [SerializeField] private AnimationConfigData _animationConfig;
        public AnimationConfigData AnimationConfig => _animationConfig;


        private void OnValidate()
        {
            _animationConfig.OnValidate();
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}