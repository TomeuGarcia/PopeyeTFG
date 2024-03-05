using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    [CreateAssetMenu(fileName = "PlayerGeneralViewConfig", 
        menuName = ScriptableObjectsHelper.PLAYER_ASSETS_PATH + "PlayerGeneralViewConfig")]
    public class PlayerGeneralViewConfig : ScriptableObject
    {
        [Header("SQUASH & STRETCH")]
        [SerializeField] private PlayerSquashStretchViewConfig _squashStretchViewConfig;
        public PlayerSquashStretchViewConfig SquashStretchViewConfig => _squashStretchViewConfig;
     
        
        [Header("MATERIAL")]
        [SerializeField] private PlayerMaterialViewConfig _materialViewConfig;
        public PlayerMaterialViewConfig MaterialViewConfig => _materialViewConfig;
     
        
        [Header("PARTICLES")]
        [SerializeField] private PlayerParticlesViewConfig _particlesViewConfig;
        public PlayerParticlesViewConfig ParticlesViewConfig => _particlesViewConfig;
        
        
        [Header("GAME FEEL EFFECTS")]
        [SerializeField] private PlayerGameFeelEffectsViewConfig _gameFeelEffectsViewConfig;
        public PlayerGameFeelEffectsViewConfig GameFeelEffectsViewConfig => _gameFeelEffectsViewConfig;
        
        
        [Header("ANIMATOR")]
        [SerializeField] private PlayerAnimatorViewConfig _animatorViewConfig;
        public PlayerAnimatorViewConfig AnimatorViewConfig => _animatorViewConfig;

    }
}