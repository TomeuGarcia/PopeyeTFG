using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations
{
    
    [CreateAssetMenu(fileName = "AimingThrowAnchor_PlayerStateConfig", 
                    menuName = PlayerStateConfigHelper.SO_ASSETS_PATH + "AimingThrowAnchor_PlayerStateConfig")]
    public class AimingThrowAnchor_PlayerStateConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 20.0f)] private float _movementSpeed = 5.0f;
        public float MovementSpeed => _movementSpeed;
    }
}