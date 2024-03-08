using DG.Tweening;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    [CreateAssetMenu(fileName = "ValueStatBarViewConfig_NAME", 
        menuName = ScriptableObjectsHelper.VALUESTATS_ASSETS_PATH + "ValueStatBarViewConfig")]
    public class ValueStatBarViewConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 10.0f)] private float _fullFillDuration = 1.0f;
        [SerializeField, Range(0.0f, 10.0f)] private float _lazyFullFillDuration = 2.0f;
        [SerializeField, Range(0.0f, 10.0f)] private float _colorPunchMinDuration = 0.4f;
    
        [SerializeField] private Color _originalColor = Color.blue;
        [SerializeField] private Color _lazyColor = Color.black;
        [SerializeField] private Color _incrementColor = Color.green;
        [SerializeField] private Color _decrementColor = Color.red;

        [SerializeField] private Ease _fillEase = Ease.InOutQuad;
        [SerializeField] private Ease _lazyFillEase = Ease.InOutQuad;
        
        
        public float FullFillDuration => _fullFillDuration;
        public float LazyFullFillDuration => _lazyFullFillDuration;
        public float ColorPunchMinDuration => _colorPunchMinDuration;
    
        public Color OriginalColor => _originalColor;
        public Color LazyColor => _lazyColor;
        public Color IncrementColor => _incrementColor;
        public Color DecrementColor => _decrementColor;

        public Ease FillEase => _fillEase;
        public Ease LazyFillEase => _lazyFillEase;
    }
}