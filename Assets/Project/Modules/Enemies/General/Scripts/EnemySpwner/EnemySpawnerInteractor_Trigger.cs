using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.WorldElements.WorldInteractors;
using Popeye.Scripts.ObjectTypes;
using UnityEngine;

namespace Popeye.Modules.Enemies.General
{
    public class EnemySpawnerInteractor_Trigger : AEnemySpawnerInteractor
    {
        [SerializeField] private Collider[] _triggers;
        [SerializeField] private AWorldInteractor[] _worldInteractors;

        
        [Header("ACCEPT TYPES")] 
        [SerializeField] private ObjectTypeAsset _playerType;

        private void Start()
        {
            if (_playerType == null)
            {
                _playerType = ServiceLocator.Instance.GetService<IObjectTypesGameService>().PlayerObjectType;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (AcceptsOtherCollider(other))
            {
                StartEnemySpawnerWaves();
            }
        }

        private bool AcceptsOtherCollider(Collider other)
        {
            if (!other.TryGetComponent(out IObjectType otherObjectType)) return false;
            return otherObjectType.IsOfType(_playerType);
        }

        protected override void OnAllEnemyWavesFinishedEvent()
        {
            foreach (AWorldInteractor worldInteractor in _worldInteractors)
            {
                worldInteractor.AddDeactivationInput();
            }
        }

        protected override void OnPlayerDiedDuringWavesEvent()
        {
            foreach (AWorldInteractor worldInteractor in _worldInteractors)
            {
                worldInteractor.AddDeactivationInput();
            }
            
            foreach (Collider trigger in _triggers)
            {
                trigger.enabled = true;
            }
        }

        protected override void OnOnFirstEnemyWaveStartedEvent()
        {
            foreach (Collider trigger in _triggers)
            {
                trigger.enabled = false;
            }

            foreach (AWorldInteractor worldInteractor in _worldInteractors)
            {
                worldInteractor.AddActivationInput();
            }
        }
    }
}
