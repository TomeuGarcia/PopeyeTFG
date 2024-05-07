using System;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.Camera.CameraZoom;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.AnimationDeath
{
    [CreateAssetMenu(fileName = "PlayerDeathAnimationSequenceConfig", 
        menuName = ScriptableObjectsHelper.PLAYER_ASSETS_PATH + "PlayerDeathAnimationSequenceConfig")]
    public class PlayerDeathAnimationSequenceConfig : ScriptableObject
    {
        [Header("CHAINS")] 
        [SerializeField] private Vector3 _chainsPositionOffset = new Vector3(0, 2, 0);
        
        [Header("PARTICLES")] 
        [SerializeField, Range(0.01f, 5.0f)] private float _deathEndDelay = 0.2f;
        [SerializeField] private string _materialsEndProperty = "_IsEnd"; 
        [SerializeField] private Material[] _deathMaterials;
        public int MaterialsEndPropertyId { get; private set; }

        [Header("CAMERA EFFECTS")] 
        [SerializeField] private CameraZoomConfig _chainAppearZoom;
        [SerializeField] private CameraZoomConfig _deathEndZoom;
        [SerializeField] private CameraShakeConfig _deathEndShake;
        
        [Header("FINISH DURATIONS")] 
        [SerializeField, Range(0.0f, 5.0f)] float _ballGrowDuration = 0.5f;
        [SerializeField, Range(0.0f, 5.0f)] float _delayFadeInDuration = 0.8f;
        [SerializeField, Range(0.0f, 5.0f)] float _fadedOutHalfDuration = 0.5f;
        
        
        public Vector3 ChainsPositionOffset => _chainsPositionOffset;
        
        public float DeathEndDelay => _deathEndDelay;
        public Material[] DeathMaterials => _deathMaterials;

        public CameraZoomConfig ChainAppearZoom => _chainAppearZoom;
        public CameraZoomConfig DeathEndZoom => _deathEndZoom;
        public CameraShakeConfig DeathEndShake => _deathEndShake;
        
        public float BallGrowDuration => _ballGrowDuration;
        public float DelayFadeInDuration => _delayFadeInDuration;
        public float FadedOutHalfDuration => _fadedOutHalfDuration;

        private void OnValidate()
        {
            MaterialsEndPropertyId = Shader.PropertyToID(_materialsEndProperty);
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}