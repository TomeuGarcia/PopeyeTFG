using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.GameMenus.Generic
{
    [CreateAssetMenu(fileName = "SmartButtonViewConfig", 
        menuName = ScriptableObjectsHelper.UIBUTTON_ASSETS_PATH + "SmartButtonViewConfig")]
    public class SmartButtonViewConfig : ScriptableObject
    {
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _highlightedColor = Color.yellow;
        [SerializeField] private Color _selectedColor = Color.yellow;
        
        public Color NormalColor => _normalColor;
        public Color HighlightedColor => _highlightedColor;
        public Color SelectedColor => _selectedColor;

    }
}