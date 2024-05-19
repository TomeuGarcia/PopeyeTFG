using System;
using NaughtyAttributes;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.GameDataEvents;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking.Checkpoint;
using Popeye.Modules.WorldElements.WorldInteractors;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.AnchorHitCheckpoint
{
    public class AnchorHitCheckpoint : MonoBehaviour, IAnchorHitCheckpointMediator
    {
        [Header("CHECKPOINT CONFIG")]
        [SerializeField] private CheckpointStorer _checkpointStorer;
        [Required()] [SerializeField] private CheckpointDataAsset _checkpointData;
        [SerializeField] private Transform _respawnSpot;

        [Header("LOGIC")]
        [SerializeField] private AnchorHitCheckpointDamageLogic _damageLogic;
        
        [Header("WORLD INTERACTORS")] 
        [SerializeField] private AWorldInteractor[] _worldInteractors;

        [Header("VIEW")]
        [SerializeField] private AnchorHitCheckpointView _view;

        private IEventSystemService _eventSystemService;
        public struct OnCheckpointSet { }
        
        

        private void Awake()
        {
            bool startsAsActiveCheckpoint = false;
            
            _checkpointData.Configure(_respawnSpot.position);
            _damageLogic.Configure(this);
            _view.Configure(startsAsActiveCheckpoint, _checkpointData.TimesUsed > 0);
        }

        private void Start()
        {
            _eventSystemService = ServiceLocator.Instance.GetService<IEventSystemService>();
        }

        private void OnEnable()
        {
            _checkpointStorer.OnLastSafeCheckpointChanged += OnLastSafeCheckpointChangedEvent;
        }
        private void OnDisable()
        {
            _checkpointStorer.OnLastSafeCheckpointChanged -= OnLastSafeCheckpointChangedEvent;
        }


        private void OnLastSafeCheckpointChangedEvent()
        {
            if (!ReferenceEquals(_checkpointStorer.LastSafeCheckpoint, _checkpointData))
            {
                _view.PlayStopBeingCurrentlyActiveCheckpoint();
            }
        }

        public void OnWasHitByAnchor(Vector3 damageSourcePosition)
        {
            _view.ComputeBounceAxis(damageSourcePosition);
            _view.PlayBounceAnimation();
            

            if (_checkpointData.TimesUsed == 0)
            {
                _view.PlayFirstTimeUsedAnimation();
                foreach (AWorldInteractor worldInteractor in _worldInteractors)
                {
                    worldInteractor.AddActivationInput();
                }
            }
            else
            {
                _view.PlayUsedAnimation();
            }
         
            if (!ReferenceEquals(_checkpointStorer.LastSafeCheckpoint, _checkpointData))
            {
                SetAsNewCheckpoint();
            }

            _eventSystemService.Dispatch(new OnCheckpointSet());
            _eventSystemService.Dispatch(new OnPlayerRestEvent(transform.position));
        }


        private void SetAsNewCheckpoint()
        {
            _checkpointStorer.SetLastSafeCheckpoint(_checkpointData);

            _view.PlayStartBeingCurrentlyActiveCheckpoint().Forget();
        }

        
    }
}