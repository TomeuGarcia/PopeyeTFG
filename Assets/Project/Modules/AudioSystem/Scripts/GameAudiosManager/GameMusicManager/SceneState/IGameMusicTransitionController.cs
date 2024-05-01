using Popeye.Scripts.Core.Scenes;

namespace Popeye.Modules.AudioSystem.GameAudiosManager
{
    public interface IGameMusicTransitionController
    {
        void TransitionToSceneMusic(ISceneReference sceneReference);
    }
}