using System;

namespace Popeye.Scripts.Core.Scenes
{
    public interface ISceneTransitionScreenFader
    {
        void FadeScreen(Func<bool> sceneFinishedLoading);
    }
}