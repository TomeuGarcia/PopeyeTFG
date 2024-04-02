using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Popeye.Scripts.TextUtilities
{
    public class TextContentInitializeHelper : MonoBehaviour
    {
        [Required()] [SerializeField] private TMP_Text _text;
        [Required()] [Expandable] [SerializeField] private TextContent _textContent;

        private void OnValidate()
        {
            if (_text && _textContent)
            {
                _text.SetContent(_textContent);    
            }            
        }

        private void Start()
        {
            _text.SetContent(_textContent);
            Destroy(this);
        }

        
    }
}