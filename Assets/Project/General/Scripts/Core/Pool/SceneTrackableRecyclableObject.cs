using Popeye.Scripts.Core.Scenes;
using Popeye.Scripts.Core.Scenes.ObjectTracking;

namespace Popeye.Core.Pool
{
    public abstract class SceneTrackableRecyclableObject : RecyclableObject, ISceneObject
    {
        private ISceneReference _belongingScene;
        private ISceneObjectTrackerListener _sceneObjectTrackerListener;
        
        private void OnDisable()
        {
            _sceneObjectTrackerListener?.OnSceneObjectDisabled(this);
        }
        
        public void SetBelongingScene(ISceneReference sceneReference)
        {
            _belongingScene = sceneReference;
        }

        public bool BelongsToScene(ISceneReference sceneReference)
        {
            return ReferenceEquals(_belongingScene, sceneReference);
        }

        public void SetTrackerListener(ISceneObjectTrackerListener sceneObjectTrackerListener)
        {
            _sceneObjectTrackerListener = sceneObjectTrackerListener;
        }

        public abstract void OnBelongSceneWasUnloaded();
    }
}