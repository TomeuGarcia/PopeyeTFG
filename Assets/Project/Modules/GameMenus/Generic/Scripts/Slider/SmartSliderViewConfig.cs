using NaughtyAttributes;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.GameMenus.Generic
{
    [CreateAssetMenu(fileName = "SmartSliderViewConfig_NAME", 
        menuName = ScriptableObjectsHelper.UISLIDER_ASSETS_PATH + "SmartSliderViewConfig")]
    public class SmartSliderViewConfig : ScriptableObject
    {
        [SerializeField] private Color _backgroundColor = Color.gray;
        [SerializeField] private Color _fillColor = Color.white;
        [SerializeField] private Color _knobColor = Color.white;

        [SerializeField] private bool _wholeNumbers = true;
        [SerializeField, MinMaxSlider(-20, 20)] private Vector2 _valueRange;

        [SerializeField] private bool _processValueTo01Range = true;
        
        
        public Color BackgroundColor => _backgroundColor;
        public Color FillColor => _fillColor;
        public Color KnobColor => _knobColor;

        public bool WholeNumbers => _wholeNumbers;
        public float MinValue => _valueRange.x;
        public float MaxValue => _valueRange.y;

        public bool ProcessValueTo01Range => _processValueTo01Range;
    }
}