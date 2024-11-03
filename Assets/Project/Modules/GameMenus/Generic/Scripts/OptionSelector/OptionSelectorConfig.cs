using NaughtyAttributes;
using Popeye.ProjectHelpers;
using Popeye.Scripts.TextUtilities;
using Project.Modules.GameMenus.Generic.Scripts.Audio;
using UnityEngine;

namespace Popeye.Modules.GameMenus.Generic
{
    [CreateAssetMenu(fileName = "OptionSelectorConfig_NAME", 
        menuName = ScriptableObjectsHelper.UIOPTIONSELECTOR_ASSETS_PATH + "OptionSelectorConfig")]
    public class OptionSelectorConfig : ScriptableObject
    {
        [Header("VIEW")]
        [Expandable] [SerializeField] private OptionSelectorViewConfig _viewConfig;
        
        [Header("AUDIO")]
        [Expandable] [SerializeField] private UIAudioInteractionConfig _optionChangedAudio;
        
        [Header("OPTIONS")]
        [Expandable] [SerializeField] private TextContent _optionNameTextContent;
        [Expandable] [SerializeField] private TextContent[] _optionTextContents;
        
        public OptionSelectorViewConfig ViewConfig => _viewConfig;
        public UIAudioInteractionConfig OptionChangedAudio => _optionChangedAudio;
        public TextContent OptionNameTextContent => _optionNameTextContent;
        public TextContent[] OptionTextContents => _optionTextContents;

    }
}
