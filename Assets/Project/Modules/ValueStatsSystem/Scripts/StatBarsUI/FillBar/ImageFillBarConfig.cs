using System;
using DG.Tweening;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    [CreateAssetMenu(fileName = "ImageFillBarConfig_NAME", 
        menuName = ScriptableObjectsHelper.VALUESTATS_ASSETS_PATH + "ImageFillBarConfig")]
    public class ImageFillBarConfig : ScriptableObject
    {
        [Header("DURATIONS")]
        [SerializeField, Range(0.0f, 10.0f)] private float _fullFillDuration = 1.0f;
        [SerializeField, Range(0.0f, 10.0f)] private float _lazyFullFillDuration = 2.0f;
        [SerializeField, Range(0.0f, 10.0f)] private float _colorPunchMinDuration = 0.4f;
    
        [Space(10)]
        [Header("COLORS")]
        [SerializeField] private Color _originalColor = Color.blue;
        [SerializeField] private Color _lazyColor = Color.black;
        [SerializeField] private Color _incrementColor = Color.green;
        [SerializeField] private Color _decrementColor = Color.red;
        [SerializeField] private Color _backgroundColor = Color.black;

        [Space(10)]
        [Header("EASE")]
        [SerializeField] private Ease _fillEase = Ease.InOutQuad;
        [SerializeField] private Ease _lazyFillEase = Ease.InOutQuad;
        
        
        public float FullFillDuration => _fullFillDuration;
        public float LazyFullFillDuration => _lazyFullFillDuration;
        public float LazyExtraDuration { get; private set; }
        
        public float ColorPunchMinDuration => _colorPunchMinDuration;
    
        public Color OriginalColor => _originalColor;
        public Color LazyColor => _lazyColor;
        public Color IncrementColor => _incrementColor;
        public Color DecrementColor => _decrementColor;
        public Color BackgroundColor => _backgroundColor;

        public Ease FillEase => _fillEase;
        public Ease LazyFillEase => _lazyFillEase;


        private void OnValidate()
        {
            LazyExtraDuration = LazyFullFillDuration - FullFillDuration;
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}