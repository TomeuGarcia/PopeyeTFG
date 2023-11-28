using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations
{
    
    [CreateAssetMenu(fileName = "MovingWithAnchor_PlayerStateConfig", 
        menuName = PlayerStateConfigHelper.SO_ASSETS_PATH + "MovingWithAnchor_PlayerStateConfig")]
    public class MovingWithAnchor_PlayerStateConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 20.0f)] private float _movementSpeed = 8.0f;
        public float MovementSpeed => _movementSpeed;
    }
}