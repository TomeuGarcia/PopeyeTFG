using System;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Scripts.Core.Scenes;
using UnityEngine;

namespace Project.Modules.GameMenus.GameCredits
{
    public class CreditsSceneController : MonoBehaviour
    {
        [Header("CREDITS")] 
        [SerializeField] private CreditsDisplayer _creditsDisplayer;

        [Header("SCENES")] 
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup _creditsFinishNextScene;

        
        private void OnEnable()
        {
            _creditsDisplayer.OnCreditsFinished += OnCreditsFinishedEvent;
        }
        private void OnDisable()
        {
            _creditsDisplayer.OnCreditsFinished -= OnCreditsFinishedEvent;
        }

        private void Start()
        {
            _creditsDisplayer.StartCredits();
        }

        private void OnCreditsFinishedEvent()
        {
            ISceneLoadManager sceneLoadManager = ServiceLocator.Instance.GetService<ISceneLoadManager>();
            
            sceneLoadManager.LoadScene(_creditsFinishNextScene);
        }
        
    }
}