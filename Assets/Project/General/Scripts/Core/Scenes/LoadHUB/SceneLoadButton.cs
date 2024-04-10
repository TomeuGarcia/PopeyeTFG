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
        private ISceneLoadManager.SceneAdditiveLoadGroup _sceneLoadGroup;
        private Action<ISceneLoadManager.SceneAdditiveLoadGroup> _buttonPressedCallback;
        
        
        public void Configure(ISceneLoadManager.SceneAdditiveLoadGroup sceneLoadGroup,
            Action<ISceneLoadManager.SceneAdditiveLoadGroup> buttonPressedCallback)
        {
            _sceneLoadGroup = sceneLoadGroup;
            _buttonPressedCallback = buttonPressedCallback;
            
            _button.onClick.AddListener(OnButtonPressed);

            _text.text = _sceneLoadGroup.SceneReference.SceneName;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }


        private void OnButtonPressed()
        {
            _buttonPressedCallback?.Invoke(_sceneLoadGroup);
        }

        public void DisableClicking()
        {
            _button.onClick.RemoveAllListeners();
        }
        
    }
}