using System;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using Popeye.Modules.WorldElements.WorldInteractors;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.WorldElements.WorldInteractors.Relay
{
    public class WorldInteractorRelayer : AWorldInteractor
    {
        [Header("DELAYS")] 
        [SerializeField, Range(0.0f, 10.0f)] private float _activationDelay = 0f;
        
        [Header("RELAY GROUPS")]
        [SerializeField] private WorldInteractorsAndRelayListener[] _interactorsAndListenerGroups;


        protected override void DoAwake()
        {
            
        }

        protected override void DoEnterActivatedState()
        {
            RelayEnterActivatedState().Forget();
        }
        
        protected override void DoEnterDeactivatedState()
        {
            RelayEnterDeactivatedState().Forget();
        }


        private async UniTaskVoid RelayEnterActivatedState()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_activationDelay));

            foreach (WorldInteractorsAndRelayListener group in _interactorsAndListenerGroups)
            {
                await group.RelayEnterActivatedState();
            }
        }
        
        private async UniTaskVoid RelayEnterDeactivatedState()
        {
            foreach (WorldInteractorsAndRelayListener group in _interactorsAndListenerGroups)
            {
                group.RelayEnterDeactivatedState();
            }
        }
        
    }
}