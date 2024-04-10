namespace Popeye.Scripts.Core.Scenes.PlayedScene
{
    public interface ICurrentlyPlayedSceneProvider
    {
        ISceneReference CurrentlyPlayedScene { get; }
    }
}