using Popeye.Core.Services.EventSystem;

namespace Popeye.Modules.AudioSystem.GameAudiosManager
{
    public interface IGameAudiosManager
    {
        void Init(IFMODAudioManager audioManager, IEventSystemService eventSystemService);
        void StartListeningToGameEvents();
        void StopListeningToGameEvents();
    }
}