using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Camera;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.Camera.CameraZoom;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.PlayerAnchor.Player;
using Popeye.Modules.PlayerAnchor.Anchor.AnchorStates;
using Popeye.Modules.PlayerAnchor.Chain;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking.OnVoid;
using Popeye.Modules.VFX.ParticleFactories;
using Project.Modules.WorldElements.DestructiblePlatforms;
using Project.Scripts.Time.TimeFunctionalities;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class PopeyeAnchor : MonoBehaviour, IAnchorMediator
    {
        [SerializeField]
        public FMODUnity.EventReference AnchorHit;
        private string AnchorHitSFX = null;

        [SerializeField]
        public FMODUnity.EventReference AnchorThrow;
        private string AnchorThrowSFX = null;

        [SerializeField]
        public FMODUnity.EventReference AnchorGrab;
        private string AnchorGrabSFX = null;

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

        public IAnchorTrajectorySnapTarget CurrentTrajectorySnapTarget { get; private set; }
        
        public IOnVoidChecker OnVoidChecker { get; private set; }

        public Vector3 Position => _anchorMotion.Position;
        public Quaternion Rotation => _anchorMotion.Rotation;

        [SerializeField] private DestructiblePlatformBreaker _destructiblePlatformBreaker;
        public DestructiblePlatformBreaker DestructiblePlatformBreaker => _destructiblePlatformBreaker;
        
        
        private IAnchorAudio _anchorAudio;

        private ICameraFunctionalities _cameraFunctionalities;
        [SerializeField] private CameraZoomInOutConfig _pull_CameraZoomInOut;
        [SerializeField] private CameraShakeConfig _restOnFloor_CameraShake;

        public void Configure(AnchorFSM stateMachine, AnchorTrajectoryMaker anchorTrajectoryMaker,
            AnchorThrower anchorThrower, AnchorPuller anchorPuller, TransformMotion anchorMotion,
            AnchorPhysics anchorPhysics, AnchorCollisions anchorCollisions, IAnchorView anchorView,
            IAnchorAudio anchorAudio,
            AnchorDamageDealer anchorDamageDealer, AnchorChain anchorChain,
            ICameraFunctionalities cameraFunctionalities,
            IOnVoidChecker onVoidChecker)
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

            _anchorAudio = anchorAudio;
            
            
            _cameraFunctionalities = cameraFunctionalities;

            OnVoidChecker = onVoidChecker;
            
            _anchorPhysics.DisableCollision();
            _anchorChain.DisableTension();
            
            _anchorView.Configure(ServiceLocator.Instance.GetService<IParticleFactory>(),
                ServiceLocator.Instance.GetService<ITimeFunctionalities>().HitStopManager,
                cameraFunctionalities.CameraShaker);
        }
        
        public void ResetState(Vector3 position)
        {
            _stateMachine.Reset();
            _anchorView.ResetView();
            SetPosition(position);
        }
        
        
        public void SetPosition(Vector3 position)
        {
            _anchorMotion.SetPosition(position);
        }

        public void SetRotation(Quaternion rotation)
        {
            _anchorMotion.SetRotation(rotation);
        }

        public void DisableChainTensionForDuration(float duration)
        {
            _anchorChain.DisableTensionForDuration(duration).Forget();
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
            _anchorChain.SetThrownView(anchorThrowResult);
            
            _anchorView.PlayThrownAnimation(anchorThrowResult.Duration);
            
            _anchorAudio.PlayThrowSound();
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
            
            _anchorAudio.PlayThrowSound();
        }
        
        
        public void SetPulled(AnchorThrowResult anchorPullResult)
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.Pulled);
            _anchorDamageDealer.DealPullDamage(anchorPullResult).Forget();
            
            /*
            _anchorMotion.MoveAlongPath(anchorPullResult.TrajectoryPathPoints, anchorPullResult.Duration, 
                AnchorPullResult.InterpolationEaseCurve);
            */
            _anchorMotion.MoveToPosition(anchorPullResult.LastTrajectoryPathPoint, anchorPullResult.Duration, 
                anchorPullResult.MoveEaseCurve);
            
            _anchorChain.SetPulledView(anchorPullResult);
            
            _anchorView.PlayPulledAnimation(anchorPullResult.Duration);

            _cameraFunctionalities.CameraZoomer.ZoomOutInToDefault(_pull_CameraZoomInOut);

        }

        public void OnDashedAt(float duration, Ease dashEase)
        {
            _anchorChain.SetDashingTowardsView(duration, dashEase);
        }
        public void OnDashedAwayFrom(float duration, Ease dashEase)
        {
            _anchorChain.SetDashingAwayView(duration, dashEase);
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
            
            _anchorAudio.PlayPickedUpSound();
        }
        public void SetGrabbedToThrow()
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.GrabbedToThrow);
        }
        public void SetRestingOnFloor()
        {
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.RestingOnFloor);
            
            _anchorView.PlayRestOnFloorAnimation();
            _anchorChain.SetRestingOnFloorView();
            
            _cameraFunctionalities.CameraShaker.PlayShake(_restOnFloor_CameraShake);
        }
        public void SetGrabbedBySnapper(IAnchorTrajectorySnapTarget anchorTrajectorySnapTarget)
        {
            CurrentTrajectorySnapTarget = anchorTrajectorySnapTarget;

            Transform parentTransform = anchorTrajectorySnapTarget.GetParentTransformForTargeter();
                
            if (parentTransform != null)
            {
                _anchorMotion.Parent(parentTransform);
            }
            
            _anchorChain.SetRestingOnFloorView();
            _stateMachine.OverwriteState(AnchorStates.AnchorStates.GrabbedBySnapper);
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

        public UniTaskVoid SnapToFloor()
        {
            throw new NotImplementedException();
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

        
        
        public async UniTaskVoid SnapToFloor(Vector3 noFloorAlternativePosition)
        {
            if (ExistsFloorUnderAnchor())
            {
                await DoSnapToFloor(Position);
                SetRestingOnFloor();
            }
            else
            {
                // idk there is no floor, reset Anchor I guess
                await DoSnapToFloor(noFloorAlternativePosition);
                SetRestingOnFloor();
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

        private async UniTask DoSnapToFloor(Vector3 floorAtPosition)
        {
            Vector3 floorPosition = PositioningHelper.Instance.GetFloorPositionUnderneath(floorAtPosition+ Vector3.up*0.5f);
            float duration = Mathf.Min(0.5f,Vector3.Distance(floorAtPosition, floorPosition) * 0.1f);

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

        public void OnDamageDealt(DamageHitResult damageHitResult)
        {
            _anchorAudio.PlayDealDamageSound();
            _anchorView.OnDamageDealt(damageHitResult);
        }
    }
}