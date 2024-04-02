using UnityEngine;

namespace Popeye.Scripts.Core.Scenes
{
    [System.Serializable]
    public class SceneLoadOptions
    {
        [SerializeField] private bool _isPersistentScene;
        [SerializeField] private bool _unloadPreviousScenes;
        [SerializeField] private bool _unloadPreviousPersistentScenes;
        [SerializeField] private bool _fadeScreen;
        
        public bool IsPersistentScene => _isPersistentScene;
        public bool UnloadPreviousScenes => _unloadPreviousScenes;
        public bool UnloadPreviousPersistentScenes => _unloadPreviousPersistentScenes;
        public bool FadeScreen => _fadeScreen;
    }
}