using System;

namespace Popeye.Scripts.Core.Scenes.ObjectTracking
{
    public interface ISceneObject
    {
        void SetBelongingScene(ISceneReference sceneReference);
        bool BelongsToScene(ISceneReference sceneReference);
        void SetTrackerListener(ISceneObjectTrackerListener sceneObjectTrackerListener);
        void OnBelongSceneWasUnloaded();
    }
}