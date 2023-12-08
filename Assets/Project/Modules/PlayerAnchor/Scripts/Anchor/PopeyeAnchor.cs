

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
        private TransformMotion _anchorMotion;

        private AnchorDamageDealer _anchorDamageDealer;
        private AnchorChain _anchorChain;

        public Vector3 Position => _anchorMotion.Position;


        public void Configure(AnchorFSM stateMachine, AnchorTrajectoryMaker anchorTrajectoryMaker,
            AnchorThrower anchorThrower, AnchorPuller anchorPuller, TransformMotion anchorMotion,
            AnchorDamageDealer anchorDamageDealer, AnchorChain anchorChain)
        {
            _stateMachine = stateMachine;
            _anchorTrajectoryMaker = anchorTrajectoryMaker;
            _anchorThrower = anchorThrower;
            _anchorPuller = anchorPuller;
            _anchorMotion = anchorMotion;

            _anchorDamageDealer = anchorDamageDealer;
            _anchorChain = anchorChain;
        }
        
        
        public void SetThrown(AnchorThrowResult anchorThrowResult)
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.Thrown);
            _anchorDamageDealer.DealThrowDamage(anchorThrowResult);
            
            _anchorChain.SetFailedThrow(anchorThrowResult.EndsOnVoid);
        }
        public void SetPulled(AnchorThrowResult anchorPullResult)
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.Pulled);
            _anchorDamageDealer.DealPullDamage(anchorPullResult);
        }
        public void SetCarried()
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.Carried);
        }
        public void SetGrabbedToThrow()
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.GrabbedToThrow);
        }
        public void SetRestingOnFloor()
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.RestingOnFloor);
        }
        public void SetGrabbedBySnapper(IAutoAimTarget autoAimTarget)
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.GrabbedBySnapper);

            
            float duration = 0.1f;
            //_anchorMotion.MoveToPosition(autoAimTarget.GetAimLockPosition(), duration, Ease.InOutSine);
            _anchorMotion.Rotate(autoAimTarget.GetRotationForAimedTargeter(), duration, Ease.InOutSine);

            Transform parentTransform = autoAimTarget.GetParentTransformForTargeter();
            if (parentTransform != null)
            {
                _anchorMotion.Parent(parentTransform);
            }
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
        public bool IsGrabbedBySnapper()
        {
            return _stateMachine.CurrentStateType == AnchorStates.AnchorStates.GrabbedBySnapper;
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


        public Vector3 GetDashEndPosition()
        {
            Vector3 dashEndPosition = Position;
            
            
            return dashEndPosition;
        }
    }
}