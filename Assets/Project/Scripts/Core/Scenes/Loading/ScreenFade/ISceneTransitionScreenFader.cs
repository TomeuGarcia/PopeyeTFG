using System;
using Cysharp.Threading.Tasks;

namespace Popeye.Scripts.Core.Scenes
{
    public interface ISceneTransitionScreenFader
    {
        UniTaskVoid FadeScreen(Func<bool> sceneFinishedLoading);
    }
}