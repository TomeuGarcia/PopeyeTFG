using System;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking.Checkpoint;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.AnchorHitCheckpoint
{
    public class AnchorHitCheckpoint : MonoBehaviour, IAnchorHitCheckpointMediator
    {
        [Header("CHECKPOINT CONFIG")]
        [SerializeField] private CheckpointStorer _checkpointStorer;
        [SerializeField] private CheckpointDataAsset _checkpointData;
        [SerializeField] private Transform _respawnSpot;

        [Header("COMPONENTS")]
        [SerializeField] private AnchorHitCheckpointDamageLogic _damageLogic;
        [SerializeField] private AnchorHitCheckpointView _view;

        [SerializeField] private GameObject _isActiveCheckpointGameObject;

        private void Awake()
        {
            _checkpointData.Configure(_respawnSpot.position);
            _damageLogic.Configure(this);

            RemoveAsActiveCheckpoint();
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
                RemoveAsActiveCheckpoint();
            }
        }

        public void OnWasHitByAnchor(Vector3 damageSourcePosition)
        {
            _view.ComputeBounceAxis(damageSourcePosition);
            _view.PlayBounceAnimation();

            if (!ReferenceEquals(_checkpointStorer.LastSafeCheckpoint, _checkpointData))
            {
                SetAsNewCheckpoint();
            }           
            
        }


        private void SetAsNewCheckpoint()
        {
            _checkpointStorer.SetLastSafeCheckpoint(_checkpointData);

            _isActiveCheckpointGameObject.SetActive(true);
        }
        
        private void RemoveAsActiveCheckpoint()
        {
            _isActiveCheckpointGameObject.SetActive(false);
        }
        
    }
}