using System;
using InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Modules.GameMenus.MainMenu
{
    public class MainMenuUnlocker : MonoBehaviour
    {
        [Header("OBJECT TO UNLOCK")] 
        [SerializeField] private GameObject _objectToUnlock;

        private InputAction _unlockInput;
        
    
        private void Awake()
        {
            _unlockInput = new PlayerAnchorInputControls().UI.Debug_UnlockMenu;
            _unlockInput.Enable();
        }

        private void Start()
        {
            Lock();
        }

        private void OnDestroy()
        {
            _unlockInput.Disable();
        }

        private void Update()
        {
            if (_unlockInput.WasPressedThisFrame())
            {
                Unlock();
                _unlockInput.Disable();
            }
        }

        private void Lock()
        {
            _objectToUnlock.SetActive(false);
        }
        private void Unlock()
        {
            _objectToUnlock.SetActive(true);
        }
    }
}