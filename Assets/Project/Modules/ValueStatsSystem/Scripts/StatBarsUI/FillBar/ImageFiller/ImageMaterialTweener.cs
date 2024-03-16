using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    public class ImageMaterialTweener : IImageTweener
    {
        private readonly Material _imageMaterial;
        private readonly Config _config;

        [System.Serializable]
        public class Config
        {
            [SerializeField] private string _fillPropertyName = "_FillValue";
            [SerializeField] private string _colorPropertyName = "_Color";
            public string FillPropertyName => _fillPropertyName;
            public string ColorPropertyName => _colorPropertyName;
            public int FillProperty { get; private set; }
            public int ColorProperty { get; private set; }

            public void Init()
            {
                FillProperty = Shader.PropertyToID(_fillPropertyName);
                ColorProperty = Shader.PropertyToID(_colorPropertyName);
            }
        }
        

        public ImageMaterialTweener(Material imageMaterial, Config config)
        {
            _imageMaterial = imageMaterial;
            _config = config;
            _config.Init();
        }

        public float FillValue => _imageMaterial.GetFloat(_config.FillProperty);

        public void SetFillValue(float value01)
        {
            _imageMaterial.SetFloat(_config.FillProperty, value01);
        }

        public void ToFillValue(float value01, float duration, Ease ease)
        {
            if (duration < SmartImage.ALMOST_ZERO_DURATION)
            {
                SetFillValue(value01);
                return;
            }
            
            _imageMaterial.DOComplete();
            _imageMaterial.DOFloat(value01, _config.FillProperty, duration)
                .SetEase(ease);
        }
        
        public void SetColor(Color color)
        {
            _imageMaterial.SetColor(_config.ColorProperty, color);
        }

        public void PunchColor(Color toColor, Color backColor, float duration)
        {
            duration /= 2;
            _imageMaterial.DOVector(toColor, _config.ColorProperty, duration)
                .OnComplete(() =>
                {
                    _imageMaterial.DOVector(backColor, _config.ColorProperty, duration);
                });
        }
    }
}