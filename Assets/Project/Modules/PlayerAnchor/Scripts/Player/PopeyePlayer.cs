using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using Project.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PopeyePlayer : MonoBehaviour, IPlayerMediator
    {
        [SerializeField] private Transform _anchorCarryHolder;
        [SerializeField] private Transform _anchorGrabToThrowHolder;
        public Transform AnchorCarryHolder => _anchorCarryHolder;
        public Transform AnchorGrabToThrowHolder => _anchorGrabToThrowHolder;
        
        private PlayerFSM _stateMachine;
        private PlayerController.PlayerController _playerController;
        private PopeyeAnchor _anchor;
        
        public Vector3 Position => _playerController.Position;
        
        public void Configure(PlayerFSM stateMachine, PlayerController.PlayerController playerController,
            PopeyeAnchor anchor)
        {
            _stateMachine = stateMachine;
            _playerController = playerController;
            _anchor = anchor;
        }


        private void Update()
        {
            _stateMachine.Update(Time.deltaTime);
        }

        
        
        public void SetMaxMovementSpeed(float maxMovementSpeed)
        {
            _playerController.MaxSpeed = maxMovementSpeed;
        }

        public void SetCanRotate(bool canRotate)
        {
            _playerController.CanRotate = canRotate;
        }

        public float GetDistanceFromAnchor()
        {
            return Vector3.Distance(Position, _anchor.Position);
        }

        public Vector3 GetFloorAlignedLookDirection()
        {
            return _playerController.GetFloorAlignedLookDirection();
        }
        public Vector3 GetFloorNormal()
        {
            return _playerController.ContactNormal;
        }

        public Vector3 GetAnchorThrowStartPosition()
        {
            return _anchorGrabToThrowHolder.position;
        }

        public void CarryAnchor()
        {
            _anchor.SetCarried();
        }
        public void AimAnchor()
        {
            _anchor.SetGrabbedToThrow();
        }

        
        public void OnAnchorThrowEndedInVoid()
        {
            _stateMachine.OverwriteState(PlayerStates.PlayerStates.PullingAnchor);
            _stateMachine.Blackboard.queuedAnchorPull = true;
        }

    }
}