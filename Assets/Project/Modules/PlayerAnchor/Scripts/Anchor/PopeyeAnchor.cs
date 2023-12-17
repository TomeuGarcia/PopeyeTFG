

using System;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Camera;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.Camera.CameraZoom;
using Popeye.Modules.PlayerAnchor.DropShadow;
using Popeye.Modules.PlayerAnchor.Player;
using Project.Modules.PlayerAnchor.Anchor.AnchorStates;
using Project.Modules.PlayerAnchor.Chain;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class PopeyeAnchor : MonoBehaviour, IAnchorMediator
    {
        private AnchorFSM _stateMachine;
        private AnchorTrajectoryMaker _anchorTrajectoryMaker;
        private AnchorThrower _anchorThrower;
        private AnchorPuller _anchorPuller;
        private TransformMotion _anchorMotion;

        private AnchorPhysics _anchorPhysics;
        private AnchorCollisions _anchorCollisions;
        private IAnchorView _anchorView;
        private AnchorDamageDealer _anchorDamageDealer;
        private AnchorChain _anchorChain;


        public Vector3 Position => _anchorMotion.Position;
        public Quaternion Rotation => _anchorMotion.Rotation;

        
        private ICameraFunctionalities _cameraFunctionalities;
        [SerializeField] private CameraZoomInOutConfig _pull_CameraZoomInOut;
        [SerializeField] private CameraShakeConfig _restOnFloor_CameraShake;

        public void Configure(AnchorFSM stateMachine, AnchorTrajectoryMaker anchorTrajectoryMaker,
            AnchorThrower anchorThrower, AnchorPuller anchorPuller, TransformMotion anchorMotion,
            AnchorPhysics anchorPhysics, AnchorCollisions anchorCollisions, IAnchorView anchorView,
            AnchorDamageDealer anchorDamageDealer, AnchorChain anchorChain,
            ICameraFunctionalities cameraFunctionalities)
        {
            _stateMachine = stateMachine;
            _anchorTrajectoryMaker = anchorTrajectoryMaker;
            _anchorThrower = anchorThrower;
            _anchorPuller = anchorPuller;
            _anchorMotion = anchorMotion;

            _anchorPhysics = anchorPhysics;
            _anchorCollisions = anchorCollisions;
            _anchorView = anchorView;
            _anchorDamageDealer = anchorDamageDealer;
            _anchorChain = anchorChain;

            _cameraFunctionalities = cameraFunctionalities;
            
            _anchorPhysics.DisableTension();
            _anchorChain.DisableTension();
        }
        
        public void ResetState()
        {
            _stateMachine.Reset();
        }
        
        
        public void SetPosition(Vector3 position)
        {
            _anchorMotion.SetPosition(position);
        }

        public void SetRotation(Quaternion rotation)
        {
            _anchorMotion.SetRotation(rotation);
        }

        
        
        public void SetThrown(AnchorThrowResult anchorThrowResult)
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.Thrown);
            _anchorDamageDealer.DealThrowDamage(anchorThrowResult);
            
            _anchorMotion.MoveAlongPath(anchorThrowResult.TrajectoryPathPoints, anchorThrowResult.Duration, 
                anchorThrowResult.MoveEaseCurve);
            _anchorMotion.RotateStartToEnd(anchorThrowResult.StartLookRotation,anchorThrowResult.EndLookRotation, 
                anchorThrowResult.Duration, anchorThrowResult.RotateEaseCurve);
            
            _anchorChain.SetFailedThrow(anchorThrowResult.EndsOnVoid);
            
            _anchorView.PlayThrownAnimation(anchorThrowResult.Duration);
        }
        
        public void SetThrownVertically(AnchorThrowResult anchorThrowResult, RaycastHit floorHit)
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.Thrown);
            _anchorDamageDealer.DealVerticalLandDamage(anchorThrowResult);
            
            _anchorMotion.MoveAlongPath(anchorThrowResult.TrajectoryPathPoints, anchorThrowResult.Duration, 
                anchorThrowResult.MoveEaseCurve);
            _anchorMotion.RotateStartToEnd(anchorThrowResult.StartLookRotation,anchorThrowResult.EndLookRotation, 
                anchorThrowResult.Duration, anchorThrowResult.RotateEaseCurve);
            
            _anchorView.PlayVerticalHitAnimation(anchorThrowResult.Duration, floorHit).Forget();
        }
        
        
        public void SetPulled(AnchorThrowResult anchorPullResult)
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.Pulled);
            _anchorDamageDealer.DealPullDamage(anchorPullResult);
            
            /*
            _anchorMotion.MoveAlongPath(anchorPullResult.TrajectoryPathPoints, anchorPullResult.Duration, 
                AnchorPullResult.InterpolationEaseCurve);
            */
            _anchorMotion.MoveToPosition(anchorPullResult.LastTrajectoryPathPoint, anchorPullResult.Duration, 
                anchorPullResult.MoveEaseCurve);
            
            _anchorView.PlayPulledAnimation(anchorPullResult.Duration);

            _cameraFunctionalities.CameraZoomer.ZoomOutInToDefault(_pull_CameraZoomInOut);

        }

        public void SetKicked(AnchorThrowResult anchorKickResult)
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.Thrown);
            _anchorDamageDealer.DealKickDamage(anchorKickResult);
            
            _anchorMotion.MoveAlongPath(anchorKickResult.TrajectoryPathPoints, anchorKickResult.Duration, 
                anchorKickResult.MoveEaseCurve);
            _anchorMotion.RotateStartToEnd(anchorKickResult.StartLookRotation,anchorKickResult.EndLookRotation, 
                anchorKickResult.Duration, anchorKickResult.RotateEaseCurve);
            
            _anchorChain.SetFailedThrow(anchorKickResult.EndsOnVoid);
            
            _anchorView.PlayKickedAnimation(anchorKickResult.Duration);
        }
        
        public void SetCarried()
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.Carried);
            
            _anchorView.PlayCarriedAnimation();
        }
        public void SetGrabbedToThrow()
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.GrabbedToThrow);
        }
        public void SetRestingOnFloor()
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.RestingOnFloor);
            
            _anchorView.PlayRestOnFloorAnimation();
            
            _cameraFunctionalities.CameraShaker.PlayShake(_restOnFloor_CameraShake);
        }
        public void SetGrabbedBySnapper(IAutoAimTarget autoAimTarget)
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.GrabbedBySnapper);

            Transform parentTransform = autoAimTarget.GetParentTransformForTargeter();
            if (parentTransform != null)
            {
                _anchorMotion.Parent(parentTransform);
            }
        }

        public void SetSpinning(bool spinningToTheRight)
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.Spinning);
            _anchorDamageDealer.StartDealingSpinDamage(spinningToTheRight);
            
            _anchorView.PlaySpinningAnimation();
        }

        public void OnKeepSpinning()
        {
            _anchorDamageDealer.UpdateSpinningDamage(Position, Rotation);
        }

        public void OnStopSpinning()
        {
            _anchorDamageDealer.StopDealingSpinDamage();
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

        public bool IsObstructedByObstacles()
        {
            return _anchorCollisions.IsObstructedByObstacles(Position, Rotation);
        }

        private bool ExistsFloorUnderAnchor()
        {
            return PositioningHelper.Instance.CheckFloorUnderneath(Position + Vector3.up*0.5f);
        }

        private async UniTask DoSnapToFloor()
        {
            Vector3 floorPosition = PositioningHelper.Instance.GetFloorPositionUnderneath(Position+ Vector3.up*0.5f);
            float duration = Vector3.Distance(Position, floorPosition) * 0.1f;

            _anchorMotion.Unparent();
            _anchorMotion.MoveToPosition(floorPosition, duration, Ease.OutSine);
            

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
        
        
        
        public void SubscribeToOnObstacleHit(Action<Collider> callback)
        {
            _anchorCollisions.SubscribeToOnObstacleHit(callback);
        }

        public void UnsubscribeToOnObstacleHit(Action<Collider> callback)
        {
            _anchorCollisions.UnsubscribeToOnObstacleHit(callback);
        }

        public void EnableObstacleHitForDuration(float duration)
        {
            _anchorCollisions.EnableObstacleHitForDuration(duration).Forget();
        }

        public void OnTryUsingWhenObstructed()
        {
            _anchorView.PlayObstructedAnimation();
        }
    }
}