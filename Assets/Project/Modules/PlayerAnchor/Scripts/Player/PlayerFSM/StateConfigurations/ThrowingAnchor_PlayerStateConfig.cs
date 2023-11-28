using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations
{
    
    [CreateAssetMenu(fileName = "ThrowingAnchor_PlayerStateConfig", 
        menuName = PlayerStateConfigHelper.SO_ASSETS_PATH + "ThrowingAnchor_PlayerStateConfig")]
    public class ThrowingAnchor_PlayerStateConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 20.0f)] private float _movementSpeed = 0.0f;
        public float MovementSpeed => _movementSpeed;
    }
}