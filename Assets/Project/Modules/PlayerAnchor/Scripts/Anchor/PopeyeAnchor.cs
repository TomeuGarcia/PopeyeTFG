

using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.PlayerAnchor.Player;
using Project.Modules.PlayerAnchor.Anchor.AnchorStates;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class PopeyeAnchor : MonoBehaviour, IAnchorMediator
    {
        private AnchorFSM _stateMachine;
        private AnchorThrowTrajectory _anchorThrowTrajectory;
        private AnchorThrower _anchorThrower;
        private AnchorPuller _anchorPuller;
        private AnchorMotion _anchorMotion;


        public Vector3 Position => _anchorMotion.AnchorPosition;


        public void Configure(AnchorFSM stateMachine, AnchorThrowTrajectory anchorThrowTrajectory,
            AnchorThrower anchorThrower, AnchorPuller anchorPuller, AnchorMotion anchorMotion)
        {
            _stateMachine = stateMachine;
            _anchorThrowTrajectory = anchorThrowTrajectory;
            _anchorThrower = anchorThrower;
            _anchorPuller = anchorPuller;
            _anchorMotion = anchorMotion;
        }
        
        
        public void SetThrown()
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.Thrown);
        }
        public void SetPulled()
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.Pulled);
        }
        public void SetCarried()
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.Carried);
        }
        public void SetGrabbedToThrow()
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.GrabbedToThrow);
        }
        private void SetRestingOnFloor()
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.RestingOnFloor);
        }
        
        
        public bool IsBeingThrown()
        {
            return _anchorThrower.AnchorIsBeingThrown();
        }
        public bool IsBeingPulled()
        {
            return _anchorPuller.AnchorIsBeingPulled();
        }
        public bool IsRestingOnFloor()
        {
            return _stateMachine.CurrentStateType == AnchorStates.AnchorStates.RestingOnFloor;
        }

        public void OnCollisionWithObstacle(Collision collision)
        {
            if (IsBeingThrown())
            {
                // interrupt throw
                _anchorThrower.InterruptThrow();
                // snap to floor
                SnapToFloor().Forget();;
            }
            else
            {
                
            }
        }


        public void OnStartChargingThrow()
        {
            _anchorThrowTrajectory.ShowTrajectoryEndSpot();
        }
        public void OnKeepChargingThrow()
        {
            _anchorThrower.UpdateThrowTrajectory();
            _anchorThrowTrajectory.UpdateTrajectoryEndSpot();
        }
        public void OnStopChargingThrow()
        {
            _anchorThrowTrajectory.HideTrajectoryEndSpot();
        }

        
        
        public async UniTaskVoid SnapToFloor()
        {
            if (ExistsFloorUnderAnchor())
            {
                await DoSnapToFloor();
                SetRestingOnFloor();
            }
            else
            {
                // idk there is no floor, reset Anchor I guess
                
            }
        }

        
        private bool ExistsFloorUnderAnchor()
        {
            return PositioningHelper.Instance.CheckFloorUnderneath(Position);
        }

        private async UniTask DoSnapToFloor()
        {
            Vector3 floorPosition = PositioningHelper.Instance.GetFloorPositionUnderneath(Position);
            float duration = Vector3.Distance(Position, floorPosition) * 0.1f;

            _anchorMotion.MoveToPosition(floorPosition, duration, Ease.OutSine);

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
        
    }
}