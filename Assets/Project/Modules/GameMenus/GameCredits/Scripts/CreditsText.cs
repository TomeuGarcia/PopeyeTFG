using System;
using Popeye.Core.Pool;
using TMPro;
using UnityEngine;

namespace Project.Modules.GameMenus.GameCredits
{
    public class CreditsText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        public RectTransform Transform => _text.rectTransform;


        public void Init(string content, TextSettings settings)
        {
            _text.text = content;
            
            _text.color = settings.Color;
            _text.font = settings.Font;
            _text.fontSize = settings.FontSize;
        }

    }
}