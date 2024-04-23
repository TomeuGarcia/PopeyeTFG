using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Modules.ValueStatSystem
{
    public class SmartImage : MonoBehaviour, IImageTweener
    {
        [SerializeField] private bool _autoInit = false;
        [SerializeField] private Image _image;
        
        
        private enum ImageMode
        {
            Component,
            Material
        }
        
        [SerializeField] private ImageMode _imageMode = ImageMode.Component;
        private IImageTweener _imageTweener;
        
        [ShowIf("_imageMode", ImageMode.Material)] 
        [SerializeField] private ImageMaterialTweener.Config _materialModeConfig;
        
        public Material ImageMaterial { get; private set; } 
        public const float ALMOST_ZERO_DURATION = 0.01f;


        private void Awake()
        {
            if (_autoInit)
            {
                Init();
            }
        }

        public void Init()
        {
            if (_imageMode == ImageMode.Component)
            {
                _imageTweener = new ImageComponentTweener(_image);
            }
            else if (_imageMode == ImageMode.Material)
            {
                ImageMaterial = new Material(_image.material);
                _image.material = ImageMaterial;
                _imageTweener = new ImageMaterialTweener(ImageMaterial, _materialModeConfig);
            }
        }


        public float FillValue => _imageTweener.FillValue;

        public void SetFillValue(float value01)
        {
            _imageTweener.SetFillValue(value01);
        }

        public void ToFillValue(float value01, float duration, Ease ease)
        {
            _imageTweener.ToFillValue(value01, duration, ease);
        }

        public void SetColor(Color color)
        {
            _imageTweener.SetColor(color);
        }

        public void PunchColor(Color toColor, Color backColor, float duration)
        {
            _imageTweener.PunchColor(toColor, backColor, duration);
        }
    }
}