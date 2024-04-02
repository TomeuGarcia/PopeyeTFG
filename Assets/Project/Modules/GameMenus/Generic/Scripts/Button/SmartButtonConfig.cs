using NaughtyAttributes;
using Popeye.ProjectHelpers;
using Popeye.Scripts.TextUtilities;
using UnityEngine;

namespace Popeye.Modules.GameMenus.Generic
{
    [CreateAssetMenu(fileName = "SmartButtonConfig_NAME", 
        menuName = ScriptableObjectsHelper.UIBUTTON_ASSETS_PATH + "SmartButtonConfig")]
    public class SmartButtonConfig : ScriptableObject
    {
        [Expandable] [SerializeField] private SmartButtonViewConfig _viewConfig;
        [Expandable] [SerializeField] private TextContent _textContent;
        
        public SmartButtonViewConfig ViewConfig => _viewConfig;
        public TextContent TextContent => _textContent;
    }
}