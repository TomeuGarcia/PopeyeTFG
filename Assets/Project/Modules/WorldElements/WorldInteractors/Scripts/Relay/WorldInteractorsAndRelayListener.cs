using AYellowpaper;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.WorldElements.WorldInteractors.Relay
{
    [System.Serializable]
    public class WorldInteractorsAndRelayListener
    {
        private enum AwaitMode
        {
            AwaitAll,
            AwaitOnlyFirst
        }
        
        
        [SerializeField] private bool _notifyListenerOnlyOnce = true;
        [SerializeField] private InterfaceReference<IWorldInteractorRelayListener, MonoBehaviour> _listener;
        [SerializeField] private AwaitMode _awaitMode = AwaitMode.AwaitOnlyFirst;
        [SerializeField] private AWorldInteractor[] _worldInteractors;
        private IWorldInteractorRelayListener Listener => _listener.Value;
        private bool IsFirstTime => _relayedTimesCounter == 0;
        private int _relayedTimesCounter;

        private bool NotifyListener => !_notifyListenerOnlyOnce || (IsFirstTime && _notifyListenerOnlyOnce);

        public async UniTask RelayEnterActivatedState()
        {
            if (NotifyListener)
            {
                await Listener.OnActivateRelayStarted();
            }
            
            
            if (_awaitMode == AwaitMode.AwaitAll)
            {
                await AwaitAllRelayEnterActivatedState();
            }
            else if (_awaitMode == AwaitMode.AwaitOnlyFirst)
            {
                await AwaitOnlyFirstRelayEnterActivatedState();
            }

            
            if (NotifyListener)
            {
                Listener.OnActivateRelayFinished();
            }
            ++_relayedTimesCounter;
        }

        
        

        private async UniTask AwaitAllRelayEnterActivatedState()
        {
            foreach (AWorldInteractor worldInteractor in _worldInteractors)
            {
                worldInteractor.EnterActivatedState();
                await worldInteractor.EnterActivatedStateAwait();
            }
        }
        private async UniTask AwaitOnlyFirstRelayEnterActivatedState()
        {
            AWorldInteractor firstWorldInteractor = _worldInteractors[0];
            firstWorldInteractor.EnterActivatedState();
            
            for (int i = 1; i < _worldInteractors.Length; ++i)
            {
                _worldInteractors[i].EnterActivatedState();
            }
            
            await firstWorldInteractor.EnterActivatedStateAwait();
        }
        
        

        public void RelayEnterDeactivatedState()
        {
            foreach (AWorldInteractor worldInteractor in _worldInteractors)
            {
                worldInteractor.EnterDeactivatedState();
            }
        }
        
    }
}