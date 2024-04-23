using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops
{
    [CreateAssetMenu(fileName = "PowerBoostDropConfig_NAME", 
        menuName = ScriptableObjectsHelper.PLAYERPOWERBOOSTDROPS_ASSETS_PATH + "PowerBoostDropConfig")]
    public class PowerBoostDropConfig : ScriptableObject
    {
        [SerializeField, Range(1, 200)] private int _experienceToDrop = 5;
        
        public int ExperienceToDrop => _experienceToDrop;
    }
}