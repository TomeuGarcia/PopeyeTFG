using Popeye.Modules.PlayerAnchor.Player.GameFeelEffects;
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
        
        
        [Header("GAME FEEL EFFECTS")]
        [SerializeField] private PlayerGameFeelEffectsConfig _gameFeelEffectsConfig;
        public PlayerGameFeelEffectsConfig GameFeelEffectsConfig => _gameFeelEffectsConfig;

    }
}