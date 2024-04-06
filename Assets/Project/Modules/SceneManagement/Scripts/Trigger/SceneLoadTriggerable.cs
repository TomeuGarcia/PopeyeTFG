using System;
using NaughtyAttributes;
using Popeye.Modules.WorldElements.Tutorial;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Popeye.Modules.SceneManagement.Scripts.Trigger
{
    public class SceneLoadTriggerable : MonoBehaviour, IWorldTriggerable
    {
        [Scene] [SerializeField] private int _sceneToLoad;
        [SerializeField] private TriggerOnceGroup _triggerOnceGroup;

        private void Awake()
        {
            _triggerOnceGroup.Init(this);
        }

        public void Activate()
        {
            SceneManager.LoadScene(_sceneToLoad);
        }

        public void Deactivate()
        {
        }
    }
}