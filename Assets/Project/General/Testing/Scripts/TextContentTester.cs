using System;
using Popeye.Scripts.Core.Scenes;
using Popeye.Scripts.TextUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Popeye.General.Testing
{
    public class TextContentTester : MonoBehaviour
    {
        [Header("CONFIGURATION")]
        [SerializeField] private AllTextContentsCollection _textContentsCollection;

        [Header("COMPONENTS")]
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _currentTranslationText;
        [SerializeField] private TextMeshProUGUI _translationText_ENG;
        [SerializeField] private TextMeshProUGUI _translationText_CAT;
        [SerializeField] private TextMeshProUGUI _translationText_ESP;

        [Header("Buttons")] 
        [SerializeField] private Button _previousButton;
        [SerializeField] private Button _nextButton;

        private int _currentTextContentIndex;

        private void Start()
        {
            _previousButton.onClick.AddListener(OnPreviousButtonPressed);
            _nextButton.onClick.AddListener(OnNextButtonPressed);
            
            _currentTextContentIndex = 0;
            UpdateTexts();
        }

        private void OnDestroy()
        {
            _previousButton.onClick.RemoveAllListeners();
            _nextButton.onClick.RemoveAllListeners();
        }

        private void OnPreviousButtonPressed()
        {
            _currentTextContentIndex = ((_currentTextContentIndex - 1) + _textContentsCollection.AllTextContents.Length) 
                                       % _textContentsCollection.AllTextContents.Length;
            UpdateTexts();
        }
        private void OnNextButtonPressed()
        {
            _currentTextContentIndex = (_currentTextContentIndex + 1) 
                                       % _textContentsCollection.AllTextContents.Length;
            UpdateTexts();   
        }

        private void UpdateTexts()
        {
            TextContent textContent = _textContentsCollection.AllTextContents[_currentTextContentIndex];

            _currentTranslationText.text = textContent.name;
            
            textContent.GetContentsByLanguage(
                out string content_ENG,
                out string content_CAT,
                out string content_ESP);

            _translationText_ENG.text = content_ENG;
            _translationText_CAT.text = content_CAT;
            _translationText_ESP.text = content_ESP;
        }
        
    }
}