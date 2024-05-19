using System;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.WorldElements.WorldInteractors.Relay
{
    [System.Serializable]
    public class WorldInteractorsAndRelayListener
    {
        private enum SequenceAwaitMode
        {
            PlayAndAwaitAllSequentially,
            PlayAllAtOnceButAwaitOnlyFirst
        }
        
        [Header("CONFIG")]
        [SerializeField] private bool _notifyListenerOnlyOnce = true;
        [SerializeField] private InterfaceReference<IWorldInteractorRelayListener, MonoBehaviour> _listener;
        [SerializeField] private SequenceAwaitMode _awaitMode = SequenceAwaitMode.PlayAllAtOnceButAwaitOnlyFirst;
        [SerializeField] private AWorldInteractor[] _worldInteractors;

        [Header("DELAYS")] 
        [SerializeField, Range(0.0f, 10.0f)] private float _delayBeforeEachWaitActivation = 0;
        [SerializeField, Range(0.0f, 10.0f)] private float _delayAfterEachWaitActivation = 0;
        
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

            if (_worldInteractors.Length < 1)
            {
                await OnlyAwait();
            }
            else if (_awaitMode == SequenceAwaitMode.PlayAndAwaitAllSequentially)
            {
                await AwaitAllRelayEnterActivatedState();
            }
            else if (_awaitMode == SequenceAwaitMode.PlayAllAtOnceButAwaitOnlyFirst)
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
                await UniTask.Delay(TimeSpan.FromSeconds(_delayBeforeEachWaitActivation), ignoreTimeScale: true);
                worldInteractor.EnterActivatedState();
                await worldInteractor.EnterActivatedStateAwait();
                await UniTask.Delay(TimeSpan.FromSeconds(_delayAfterEachWaitActivation), ignoreTimeScale: true);
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
                
            await UniTask.Delay(TimeSpan.FromSeconds(_delayBeforeEachWaitActivation), ignoreTimeScale: true);
            await firstWorldInteractor.EnterActivatedStateAwait();
            await UniTask.Delay(TimeSpan.FromSeconds(_delayAfterEachWaitActivation), ignoreTimeScale: true);
        }

        private async UniTask OnlyAwait()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_delayBeforeEachWaitActivation + _delayAfterEachWaitActivation), 
                ignoreTimeScale: true);
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