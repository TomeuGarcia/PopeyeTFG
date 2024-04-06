using NaughtyAttributes;
using Popeye.ProjectHelpers;
using Popeye.Scripts.TextUtilities;
using UnityEngine;

namespace Popeye.Modules.GameMenus.Generic
{
    [CreateAssetMenu(fileName = "SmartSliderConfig_NAME", 
        menuName = ScriptableObjectsHelper.UISLIDER_ASSETS_PATH + "SmartSliderConfig")]
    public class SmartSliderConfig : ScriptableObject
    {
        [Expandable] [SerializeField] private SmartSliderViewConfig _viewConfig;
        public SmartSliderViewConfig ViewConfig => _viewConfig;


        [Expandable] [SerializeField] private TextContent _textContent;
        public TextContent TextContent => _textContent;

    }
}