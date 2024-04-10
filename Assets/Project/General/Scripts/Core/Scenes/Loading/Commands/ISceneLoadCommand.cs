using Popeye.Core.Services.CommandQueue;

namespace Popeye.Scripts.Core.Scenes
{
    public interface ISceneLoadCommand : ICommand
    {
        bool FinishedLoading { get; }
    }
}