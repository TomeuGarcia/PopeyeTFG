using Popeye.Scripts.Core.Scenes;

namespace Popeye.Modules.GameState
{
    public interface IGameStateEventsDispatcher
    {
        public readonly struct OnGamePaused{}
        public readonly struct OnGameResumed{}
        public readonly struct OnExitToMainMenu{}

        void InvokeOnGamePaused();
        void InvokeOnGameResumed();
        void InvokeOnExitToMainMenu();
        
        
        
        public readonly struct OnStartLoadingAdditiveScene
        {
            private readonly ISceneReference _sceneReference;
            public ISceneReference SceneReference => _sceneReference;

            public OnStartLoadingAdditiveScene(ISceneReference sceneReference)
            {
                _sceneReference = sceneReference;
            }
        }

        public readonly struct OnFinishLoadingScenes { }

        public readonly struct OnStartUnloadingScene
        {
            private readonly ISceneReference _sceneReference;
            public ISceneReference SceneReference => _sceneReference;
            
            public OnStartUnloadingScene(ISceneReference sceneReference)
            {
                _sceneReference = sceneReference;
            }
        }

        void InvokeOnStartLoadingAdditiveScene(ISceneReference sceneReference);
        void InvokeOnFinishLoadingScenes();
        void InvokeOnStartUnloadingScene(ISceneReference sceneReference);
        
    }
}