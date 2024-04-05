using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Scripts.Core.Scenes
{
    public class SceneLoadButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;
        private ISceneLoadManager _sceneLoadManager;
        private ISceneLoadManager.SceneAdditiveLoadGroup _sceneLoadGroup;
        private Action _buttonPressedCallback;
        
        
        public void Configure(ISceneLoadManager sceneLoadManager, ISceneLoadManager.SceneAdditiveLoadGroup sceneLoadGroup,
            Action buttonPressedCallback)
        {
            _sceneLoadManager = sceneLoadManager;
            _sceneLoadGroup = sceneLoadGroup;
            _buttonPressedCallback = buttonPressedCallback;
            
            _button.onClick.AddListener(OnButtonPressed);

            _text.text = _sceneLoadGroup.sceneReference.SceneName;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }


        private void OnButtonPressed()
        {
            _sceneLoadManager.LoadSceneAdditively(_sceneLoadGroup);
            _buttonPressedCallback?.Invoke();
        }

        public void DisableClicking()
        {
            _button.onClick.RemoveAllListeners();
        }
        
    }
}