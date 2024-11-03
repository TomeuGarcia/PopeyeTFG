using System;
using Popeye.Scripts.Core.Scenes;
using Popeye.Scripts.TextUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif


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
        [SerializeField] private Button _findTextContentAssetButton;

        [Header("Other")] 
        [SerializeField] private GameObject _incompleteTranslationWarning;

        private int _currentTextContentIndex;

        private TextContent CurrentTextContent => _textContentsCollection.AllTextContents[_currentTextContentIndex];
        private int AllTextContentsCount => _textContentsCollection.AllTextContents.Length;
        
        private void Start()
        {
            _previousButton.onClick.AddListener(OnPreviousButtonPressed);
            _nextButton.onClick.AddListener(OnNextButtonPressed);
            _findTextContentAssetButton.onClick.AddListener(OnFindTextContentAssetPressed);

            TextContent.OnContentUpdated += OnTextContentUpdated;
            
            _currentTextContentIndex = 0;
            UpdateTexts();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _previousButton.onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _nextButton.onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                _findTextContentAssetButton.onClick.Invoke();
            }
        }

        private void OnDestroy()
        {
            _previousButton.onClick.RemoveAllListeners();
            _nextButton.onClick.RemoveAllListeners();
            _findTextContentAssetButton.onClick.RemoveAllListeners();
            
            TextContent.OnContentUpdated -= OnTextContentUpdated;
        }

        private void OnPreviousButtonPressed()
        {
            _currentTextContentIndex = ((_currentTextContentIndex - 1) + AllTextContentsCount) 
                                       % AllTextContentsCount;
            UpdateTexts();
        }
        private void OnNextButtonPressed()
        {
            _currentTextContentIndex = (_currentTextContentIndex + 1) 
                                       % AllTextContentsCount;
            UpdateTexts();   
        }

        private void UpdateTexts()
        {
            TextContent textContent = CurrentTextContent;

            _currentTranslationText.text = textContent.name + $"\n{_currentTextContentIndex+1}/{AllTextContentsCount}";
            
            textContent.GetContentsByLanguage(
                out string content_ENG,
                out string content_CAT,
                out string content_ESP);

            _translationText_ENG.text = content_ENG;
            _translationText_CAT.text = content_CAT;
            _translationText_ESP.text = content_ESP;
            
            _incompleteTranslationWarning.SetActive(!textContent.HasAllFieldsCompleted());
        }

        private void OnTextContentUpdated(TextContent textContent)
        {
            if (textContent == CurrentTextContent)
            {
                UpdateTexts();
            }
        }
        
        private void OnFindTextContentAssetPressed()
        {
#if UNITY_EDITOR
            EditorUtility.FocusProjectWindow();
            TextContent textContent = AssetDatabase.LoadAssetAtPath<TextContent>(AssetDatabase.GetAssetPath(CurrentTextContent));
            Selection.activeObject = textContent;
#endif
        }
    }
}