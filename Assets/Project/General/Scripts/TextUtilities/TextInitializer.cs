using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Popeye.Scripts.TextUtilities
{
    public class TextInitializer : MonoBehaviour
    {
        [Required()] [SerializeField] private TMP_Text _text;
        [Required()] [Expandable] [SerializeField] private TextContent _textContent;

        private void OnValidate()
        {
            if (_text && _textContent)
            {
                UpdateText();    
            }            
        }

        private void Start()
        {
            UpdateText();
            Destroy(this);
        }

        [Button()]
        private void UpdateText()
        {
            _text.SetContent(_textContent);
        }
        
    }
}