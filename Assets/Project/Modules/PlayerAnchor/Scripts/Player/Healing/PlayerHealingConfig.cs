using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    [CreateAssetMenu(fileName = "PlayerHealingConfig_NAME", 
        menuName = ScriptableObjectsHelper.PLAYER_ASSETS_PATH + "PlayerHealingConfig")]
    public class PlayerHealingConfig : ScriptableObject
    {
        [SerializeField, Range(1, 300)] private int _potionHealAmount = 30;
        [SerializeField, Range(1, 10)] private int _numberOfHeals = 3;
        public int PotionHealAmount => _potionHealAmount;
        public int NumberOfHeals => _numberOfHeals;
    }
}