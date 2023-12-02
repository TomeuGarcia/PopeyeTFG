using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations
{
    [CreateAssetMenu(fileName = "PlayerMovesetConfig", 
        menuName = "Popeye/PlayerAnchor/Player/PlayerMovesetConfig")]
    public class PlayerMovesetConfig : ScriptableObject
    {
        [Header("DASH")]
        [SerializeField] private Vector3 _dashExtraDisplacement = new Vector3(0.0f, 1.0f, 0.5f);
        [SerializeField] private Vector3 _snapExtraDisplacement = new Vector3(0.0f, 1.0f, 1.0f);

        public Vector3 DashExtraDisplacement => _dashExtraDisplacement;
        public Vector3 SnapExtraDisplacement => _snapExtraDisplacement;

    }
}