using Cysharp.Threading.Tasks;

namespace Popeye.Modules.WorldElements.WorldInteractors.Relay
{
    public interface IWorldInteractorRelayListener
    {
        UniTask OnActivateRelayStarted();
        void OnActivateRelayFinished();        
    }
}