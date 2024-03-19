using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Popeye.Modules.GameMenus.Generic
{
    public abstract class AMenuController : MonoBehaviour
    {
        [Header("GROUP")]
        [SerializeField] private GameObject _groupHolder;
        
        [Header("CLOSE")]
        [SerializeField] private SmartButtonAndConfig _backButtonAndConfig;


        private InputAction _goBackInput;

        private bool IsBeingShown => _groupHolder.activeInHierarchy;
        

        public void Init(SmartButton.SmartButtonEvent closeMenuCallback, InputAction goBackInput)
        {
            _backButtonAndConfig.SmartButton.Init(_backButtonAndConfig.Config, closeMenuCallback);
            _goBackInput = goBackInput;
            
            DoInit(goBackInput);
        }

        protected abstract void DoInit(InputAction goBackInput);

        public void Show()
        {
            _groupHolder.SetActive(true);
        }
        
        public void Hide()
        {
            _groupHolder.SetActive(false);
        }

        private void Update()
        {
            if (_goBackInput.WasPressedThisFrame() && IsBeingShown)
            {
                _backButtonAndConfig.SmartButton.SimulateOnButtonClicked();
            }
        }
    }
}