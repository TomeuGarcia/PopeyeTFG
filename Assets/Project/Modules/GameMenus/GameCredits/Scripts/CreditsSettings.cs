using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Modules.GameMenus.GameCredits
{
    [CreateAssetMenu(fileName = "CreditsSettings", 
        menuName = ScriptableObjectsHelper.GAMEMENU_ASSETS_PATH + "CreditsSettings")]
    public class CreditsSettings : ScriptableObject
    {
        [SerializeField] private TextSettings _creditsTitleSettings;
        [SerializeField] private TextSettings _blockSettings;
        [SerializeField] private TextSettings _blockElementSettings;
        [SerializeField] private TextSettings _itemSettings;
    
        public TextSettings CreditsTitleSettings => _creditsTitleSettings;
        public TextSettings BlockSettings => _blockSettings;
        public TextSettings BlockElementSettings => _blockElementSettings;
        public TextSettings ItemSettings => _itemSettings;
    }
}