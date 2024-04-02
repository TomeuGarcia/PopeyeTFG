namespace Popeye.Scripts.Core.Scenes
{
    public interface ISceneLoader
    {
        void LoadSceneAdditively(SceneReferenceAsset sceneReference, SceneLoadOptions loadOptions);
        void UnloadScene(SceneReferenceAsset sceneReference);
    }
}