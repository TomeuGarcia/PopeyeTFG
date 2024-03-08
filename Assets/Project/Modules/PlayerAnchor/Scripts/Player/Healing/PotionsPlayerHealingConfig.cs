using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    [CreateAssetMenu(fileName = "PotionsPlayerHealingConfig", 
        menuName = ScriptableObjectsHelper.PLAYER_ASSETS_PATH + "PotionsPlayerHealingConfig")]
    public class PotionsPlayerHealingConfig : ScriptableObject
    {
        [SerializeField, Range(1, 300)] private int _potionHealAmount = 30;
        [SerializeField, Range(1, 10)] private int _numberOfHeals = 3;
        public int PotionHealAmount => _potionHealAmount;
        public int NumberOfHeals => _numberOfHeals;
    }
}