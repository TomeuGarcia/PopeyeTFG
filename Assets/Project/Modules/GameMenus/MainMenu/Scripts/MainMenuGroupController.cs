using NaughtyAttributes;
using Popeye.Modules.GameMenus.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Modules.GameMenus.MainMenu.Scripts
{
    public class MainMenuGroupController : MonoBehaviour
    {
        [Header("BUTTONS")]
        [SerializeField] private SmartButtonAndConfig _playButtonAndConfig;
        [SerializeField] private SmartButtonAndConfig _quitButtonAndConfig;

        [Header("GAME SCENE")]
        [Scene] [SerializeField] private int _gameScene; 
        
        private void Start()
        {
            _playButtonAndConfig.SmartButton.Init(_playButtonAndConfig.Config, PlayGame);
            _quitButtonAndConfig.SmartButton.Init(_quitButtonAndConfig.Config, QuitGame);
        }


        private void PlayGame()
        {
            SceneManager.LoadScene(_gameScene);
        }
        
        private void QuitGame()
        {
            Application.Quit();
        }
        
    }
}