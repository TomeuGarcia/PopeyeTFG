using System;
using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Core.Services.InformationDisplay
{
    public class ImageTextDisplayViewEffects : MonoBehaviour, ITextDisplayViewEffects
    {
        [SerializeField] private Image _effectImage;
        [SerializeField, Range(0f, 1f)] private float _imageColorAlphaOverride = 0.8f;


        public void UpdateView(TextDisplaySettings textDisplaySettings)
        {
            Color imageColor = textDisplaySettings.HeaderColor;
            imageColor.a = _imageColorAlphaOverride * 255;
            _effectImage.color = imageColor;
        }
        
    }
}