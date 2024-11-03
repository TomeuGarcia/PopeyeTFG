using NaughtyAttributes;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.GameMenus.Generic
{
    [CreateAssetMenu(fileName = "OptionSelectorViewConfig_NAME", 
        menuName = ScriptableObjectsHelper.UIOPTIONSELECTOR_ASSETS_PATH + "OptionSelectorViewConfig")]
    public class OptionSelectorViewConfig : ScriptableObject
    {
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _highlightedColor = Color.yellow;
        [SerializeField] private Color _selectedColor = Color.yellow;
        
        public Color NormalColor => _normalColor;
        public Color HighlightedColor => _highlightedColor;
        public Color SelectedColor => _selectedColor;
    }
}