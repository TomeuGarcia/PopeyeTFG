using System;
using DG.Tweening;
using NaughtyAttributes;
using Popeye.ProjectHelpers;
using Project.Scripts.TweenExtensions;
using UnityEngine;
using UnityEngine.Serialization;

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
        
        
        
        [Space(10)] 
        [SerializeField] private bool _punchScale = false;
        [FormerlySerializedAs("_incrementPunchScaleConfig")] [ShowIf("_punchScale")] [Expandable] [SerializeField] private TweenPunchConfigAsset incrementPunchScaleConfigAsset;
        [FormerlySerializedAs("_decrementPunchScaleConfig")] [ShowIf("_punchScale")] [Expandable] [SerializeField] private TweenPunchConfigAsset decrementPunchScaleConfigAsset;

        public bool PunchScale => _punchScale;
        public TweenPunchConfigAsset IncrementPunchScaleConfigAsset => incrementPunchScaleConfigAsset;
        public TweenPunchConfigAsset DecrementPunchScaleConfigAsset => decrementPunchScaleConfigAsset;


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