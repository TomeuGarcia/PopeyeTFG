

using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.PlayerAnchor.Player;
using Project.Modules.PlayerAnchor.Anchor.AnchorStates;
using Project.Modules.PlayerAnchor.Chain;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class PopeyeAnchor : MonoBehaviour, IAnchorMediator
    {
        private AnchorFSM _stateMachine;
        private AnchorTrajectoryMaker _anchorTrajectoryMaker;
        private AnchorThrower _anchorThrower;
        private AnchorPuller _anchorPuller;
        private AnchorMotion _anchorMotion;

        private AnchorChain _anchorChain;

        public Vector3 Position => _anchorMotion.AnchorPosition;


        public void Configure(AnchorFSM stateMachine, AnchorTrajectoryMaker anchorTrajectoryMaker,
            AnchorThrower anchorThrower, AnchorPuller anchorPuller, AnchorMotion anchorMotion,
            AnchorChain anchorChain)
        {
            _stateMachine = stateMachine;
            _anchorTrajectoryMaker = anchorTrajectoryMaker;
            _anchorThrower = anchorThrower;
            _anchorPuller = anchorPuller;
            _anchorMotion = anchorMotion;

            _anchorChain = anchorChain;
        }
        
        
        public void SetThrown(bool failedThrow)
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.Thrown);
            
            _anchorChain.SetFailedThrow(failedThrow);
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
            _anchorTrajectoryMaker.ShowTrajectoryEndSpot();
        }
        public void OnKeepChargingThrow()
        {
            _anchorThrower.UpdateThrowTrajectory();
        }
        public void OnStopChargingThrow()
        {
            _anchorTrajectoryMaker.HideTrajectoryEndSpot();
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