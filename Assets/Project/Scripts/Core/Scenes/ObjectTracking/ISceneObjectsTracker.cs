namespace Popeye.Scripts.Core.Scenes.ObjectTracking
{
    public interface ISceneObjectsTracker
    {
        void StartListeningToSceneUpdates();
        void StopListeningToSceneUpdates();
        void StartTrackingObject(ISceneObject sceneObject);
    }
}