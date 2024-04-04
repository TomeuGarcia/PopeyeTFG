namespace Popeye.Scripts.Core.Scenes
{
    public interface ISceneLoadManager
    {
        [System.Serializable]
        public struct SceneAdditiveLoadGroup
        {
            public SceneReferenceAsset sceneReference;
            public SceneLoadOptionsAsset loadOptions;
        }
        
        void LoadSceneAdditively(SceneAdditiveLoadGroup sceneLoadGroup);
        void LoadScene(SceneAdditiveLoadGroup sceneLoadGroup);
        void UnloadScene(SceneReferenceAsset sceneReference);
    }
}