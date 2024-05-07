using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Modules.GameMenus.GameCredits
{
    [CreateAssetMenu(fileName = "CreditsDisplayConfig", 
        menuName = ScriptableObjectsHelper.GAMEMENU_ASSETS_PATH + "CreditsDisplayConfig")]
    public class CreditsDisplayConfig : ScriptableObject
    {
        [Header("REFERENCES")]
        [SerializeField] private CreditsContent _content;
        [SerializeField] private CreditsSettings _settings;
        [SerializeField] private CreditsText _textPrefab;

        [Header("SCROLLING")] 
        [SerializeField] private float _normalScrollSpeed = 50.0f;
        
        
        public CreditsContent Content => _content;        
        public CreditsSettings Settings => _settings;
        public CreditsText TextPrefab => _textPrefab;
        
        
        public float NormalScrollSpeed => _normalScrollSpeed;
    }
}