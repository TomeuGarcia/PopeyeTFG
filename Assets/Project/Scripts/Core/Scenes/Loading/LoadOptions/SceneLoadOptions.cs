using UnityEngine;

namespace Popeye.Scripts.Core.Scenes
{
    [System.Serializable]
    public class SceneLoadOptions
    {
        [Header("TRANSITION")]
        [SerializeField] private bool _fadeScreen = true;
        [SerializeField] private float _delayBeforeLoading;
        
        [Header("STORE TYPE")]
        [SerializeField] private bool _isPersistentScene;

        [Header("UNLOAD")]
        [SerializeField] private bool _unloadPreviousNonPersistentScenes;
        [SerializeField] private bool _unloadPreviousPersistentScenes;

        
        public bool FadeScreen => _fadeScreen;
        public float DelayBeforeLoading => _delayBeforeLoading;
        
        public bool IsPersistentScene => _isPersistentScene;
        
        public bool UnloadPreviousNonPersistentScenes => _unloadPreviousNonPersistentScenes;
        public bool UnloadPreviousPersistentScenes => _unloadPreviousPersistentScenes;
    }
}