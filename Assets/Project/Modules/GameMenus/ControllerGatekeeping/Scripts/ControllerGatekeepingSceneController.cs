using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using InputSystem;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Scripts.Core.Scenes;
using Project.Scripts.TweenExtensions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Project.Modules.GameMenus.ControllerGatekeeping
{
    public class ControllerGatekeepingSceneController : MonoBehaviour
    {
        [Header("SCENE")]
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup _nextScene;

        [Header("IMAGE")] 
        [SerializeField] private Image _controllerImage;
        [SerializeField] private TweenPunchConfig _continueScalePunch;
        [SerializeField] private Color _continueColor;
    
        private InputAction _confirmInput;
        private InputAction _quitInput;
        
        private void Awake()
        {
            PlayerAnchorInputControls.UIActions uiActions = new PlayerAnchorInputControls().UI;
            _confirmInput = uiActions.Accept;
            _quitInput = uiActions.Quit;
            
            _confirmInput.Enable();
            _quitInput.Enable();
        }

        private void OnDestroy()
        {
            DisableInputs();
        }

        private void Update()
        {
            if (_confirmInput.WasPressedThisFrame())
            {
                ContinueToNextScene().Forget();
            }
            else if (_quitInput.WasPressedThisFrame())
            {
                Application.Quit();
            }
        }


        private async UniTaskVoid ContinueToNextScene()
        {
            DisableInputs();

            _controllerImage.DOColor(_continueColor, _continueScalePunch.Duration).SetEase(_continueScalePunch.Ease);
            await _controllerImage.rectTransform.PunchScale(_continueScalePunch)
                .AsyncWaitForCompletion();

            ISceneLoadManager sceneLoadManager = ServiceLocator.Instance.GetService<ISceneLoadManager>();
            
            sceneLoadManager.LoadScene(_nextScene);
        }

        private void DisableInputs()
        {
            _confirmInput.Disable();
            _quitInput.Disable();
        }
        
    }
}