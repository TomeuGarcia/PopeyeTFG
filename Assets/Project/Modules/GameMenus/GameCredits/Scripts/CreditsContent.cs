using NaughtyAttributes;
using Popeye.ProjectHelpers;
using Popeye.Scripts.TextUtilities;
using UnityEngine;

namespace Project.Modules.GameMenus.GameCredits
{
    [CreateAssetMenu(fileName = "CreditsContent", 
        menuName = ScriptableObjectsHelper.GAMEMENU_ASSETS_PATH + "CreditsContent")]
    public class CreditsContent : ScriptableObject
    {
        [Header("IMAGES")] 
        [SerializeField] private Sprite _gameLogo;
        [SerializeField] private Sprite _teamLogo;

        [Header("CLOSING")]
        [SerializeField] private TextContent _closing;
        
        [Header("TEXTS")]
        [Expandable] [SerializeField] private TextContent _titleText;
        [SerializeField] private float _extraHeightGap = 300f;
        [SerializeField] private CreditsBlock[] _blocks;
        

        public Sprite GameLogo => _gameLogo;
        public Sprite TeamLogo => _teamLogo;
        
        public string Title => _titleText.Content;
        public float ExtraHeightGap => _extraHeightGap;
        public CreditsBlock[] Blocks => _blocks;
        
        public string Closing => _closing.Content;
    }
}