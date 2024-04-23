using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.WorldElements.Tutorial;
using Popeye.Scripts.Core.Scenes;
using UnityEngine;


namespace Popeye.Modules.SceneManagement.Scripts.Trigger
{
    public class SceneLoadTriggerable : MonoBehaviour, IWorldTriggerable
    {
        [Header("TRIGGER")]
        [SerializeField] private TriggerOnceGroup _triggerOnceGroup;

        [Header("SCENE LOADING")] 
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup _sceneLoadGroup;

        private ISceneLoadManager _sceneLoadManager;
        
        
        private void Awake()
        {
            _triggerOnceGroup.Init(this);
        }

        private void Start()
        {
            _sceneLoadManager = ServiceLocator.Instance.GetService<ISceneLoadManager>();
        }

        public void Activate()
        {
            _sceneLoadManager.LoadSceneAdditively(_sceneLoadGroup);
        }

        public void Deactivate()
        {
        }
    }
}