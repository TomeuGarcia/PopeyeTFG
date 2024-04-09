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