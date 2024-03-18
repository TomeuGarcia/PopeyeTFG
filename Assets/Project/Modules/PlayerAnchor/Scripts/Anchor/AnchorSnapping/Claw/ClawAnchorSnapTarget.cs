using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Popeye.Modules.PlayerController.AutoAim;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class ClawAnchorSnapTarget : MonoBehaviour, IAnchorSnapTarget, IAutoAimTarget
    {
        [Header("CONFIGURATION")]
        [Expandable] [SerializeField] private ClawAnchorSnapTargetConfig _config;
        [Expandable] [SerializeField] private AutoAimTargetDataConfig _autoAimTargetDataConfig;
        
        [Space(20)]
        [Header("LOGIC Reference")]
        [SerializeField] private Transform _dashEndSpot;
        [SerializeField] private Transform _snapSpot;
        
        [Header("VIEW Reference")]
        [SerializeField] private Transform _clawsTransform;
        [SerializeField] private Transform[] _claws;

        ClawAnchorSnapTargetView _view;

        private Transform _user;
        
        

        
        public AutoAimTargetDataConfig DataConfig => _autoAimTargetDataConfig;
        public Vector3 Position => GetAimLockPosition();
        public GameObject GameObject => gameObject;
        
        private Vector3 LookDirection => transform.up;
        private Vector3 UpDirection => -transform.right;

        private float FloorProbeDistance => _config.FloorCollisionProbingConfig.ProbeDistance;
        private LayerMask FloorCollisionLayerMask => _config.FloorCollisionProbingConfig.CollisionLayerMask;
        private QueryTriggerInteraction FloorCollisionQueryTriggerInteraction => _config.FloorCollisionProbingConfig.QueryTriggerInteraction;

        private float HeightDistanceFromFloor => _config.HeightDistanceFromFloor;
        private float ForwardDistanceFromClaw => _config.ForwardDistanceFromClaw;

        private float MinDotToAcceptUser => _config.MinDotToAcceptUser;
        private float HeightDistanceToAcceptUser => _config.HeightDistanceToAcceptUser;
        

        public delegate void UsedEvents();

        public UsedEvents OnStartBeingUsedEvent;
        public UsedEvents OnStopBeingUsedEvent;

        private void Awake()
        {
            _view = new ClawAnchorSnapTargetView(_clawsTransform, _claws, _config.ViewConfig);
        }


        public Transform GetParentTransformForTargeter()
        {
            return _snapSpot;
        }

        public Vector3 GetAimLockPosition()
        {
            return _snapSpot.position;
        }

        public Vector3 GetLookDirectionForAimedTargeter()
        {
            return LookDirection;
        }

        public bool CanBeAimedFromPosition(Vector3 position)
        {
            Vector3 aimToLockPosition = position - GetAimLockPosition();
            if (Mathf.Abs(aimToLockPosition.y) > HeightDistanceToAcceptUser)
            {
                return false;
            }
            
            Vector3 direction = aimToLockPosition.normalized;
            float dot = Vector3.Dot(direction, LookDirection);

            return dot > MinDotToAcceptUser;
        }

        [Button("Correct Dash End Position")]
        private void CorrectDashEndPosition()
        {
            Vector3 origin = Position + (Vector3.up * FloorProbeDistance / 2);
            Vector3 wallDirection = LookDirection * (ForwardDistanceFromClaw > 0 ? 1 : -1);
            
            if (Physics.Raycast(origin, wallDirection, out RaycastHit wallHit, Mathf.Abs(ForwardDistanceFromClaw),
                    FloorCollisionLayerMask, FloorCollisionQueryTriggerInteraction))
            {
                origin = wallHit.point - (wallDirection * 1.0f);
            }
            else
            {
                origin += (LookDirection * ForwardDistanceFromClaw);
            }
            
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit floorHit, FloorProbeDistance,
                    FloorCollisionLayerMask, FloorCollisionQueryTriggerInteraction))
            {
                _dashEndSpot.position = floorHit.point + (Vector3.up * HeightDistanceFromFloor);
            }
            else
            {
                Debug.Log("kekewait");
            }
        }
        
        public Vector3 GetDashEndPosition()
        {
            return _dashEndSpot.position;
        }

        public Vector3 GetUserPosition()
        {
            return _user.position;
        }

        public bool CanBeAimedAt(Vector3 aimFromPosition)
        {
            return CanBeAimedFromPosition(aimFromPosition);
        }
        
        public Vector3 GetLookDirection()
        {
            return LookDirection;
        }
        

        public Quaternion GetRotationForAimedTargeter()
        {
            return Quaternion.AngleAxis(45.0f, LookDirection) * Quaternion.LookRotation(-LookDirection, UpDirection);
        }
        
        
        public void OnAddedAsAimTarget()
        {
            _view.PlayOpenAnimation();
        }

        public void OnRemovedFromAimTarget()
        {
            _view.PlayCloseAnimation();
        }

        public void OnUsedAsAimTarget(float delay)
        {
            _view.PlaySnapAnimation(delay).Forget();
        }

        public void OnStartBeingUsed(Transform user)
        {
            _user = user;
            OnStartBeingUsedEvent?.Invoke();
        }
        public void OnFinishBeingUsed()
        {
            _user = null;
            OnStopBeingUsedEvent?.Invoke();
        }




    }
}