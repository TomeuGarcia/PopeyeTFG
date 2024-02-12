using System;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Popeye.Modules.PlayerAnchor.Anchor;
using Popeye.Timers;
using UnityEngine;

namespace Project.Modules.WorldElements.MovableBlocks.PullableBlocks
{
    public class AnchorSnapTargetPullHandle : MonoBehaviour, IPullableBlockPullHandle
    {

        [Expandable] [SerializeField] private PullableBlockPullHandleConfig _config;
        [SerializeField] private ClawAnchorSnapTarget _anchorSnapTarget;
        [SerializeField] private bool _directionAlwaysMatchesClaw = true;
        [ShowIf("UsingInputDirection")] [SerializeField] private Vector2 _pullDirection;

        private IPullableBlock _pullableBlock;
        private bool _checkPulling;
        private Timer _pullingTimer;
        
        
        private float RequiredDistanceToPull => _config.RequiredDistanceToPull;
        private float RequiredTimePulling => _config.RequiredTimePulling;
        private float RequiredDotForPulling => _config.RequiredDotForPulling;

        private bool UsingInputDirection => !_directionAlwaysMatchesClaw;

        private void OnValidate()
        {
            if (_anchorSnapTarget != null && _pullDirection.sqrMagnitude < 0.1f)
            {
                UpdatePullDirectionWithClawDirection();
            }
        }

        [ShowIf("UsingInputDirection")] [Button("Match Direction With Claw")]
        private void UpdatePullDirectionWithClawDirection()
        {
            Vector3 snapTargetLookDirection = _anchorSnapTarget.GetLookDirection();
            _pullDirection = new Vector2(
                Mathf.Ceil(snapTargetLookDirection.x - 0.1f),
                Mathf.Ceil(snapTargetLookDirection.z - 0.1f));
        }

        private void OnEnable()
        {
            _anchorSnapTarget.OnStartBeingUsedEvent += StartCheckPulling;
            _anchorSnapTarget.OnStopBeingUsedEvent += StopCheckPulling;
        }
        private void OnDisable()
        {
            _anchorSnapTarget.OnStartBeingUsedEvent -= StartCheckPulling;
            _anchorSnapTarget.OnStopBeingUsedEvent -= StopCheckPulling;
        }

        private void Update()
        {
            UpdateCheckPulling();
        }
        
        public void Configure(IPullableBlock pullableBlock)
        {
            _pullableBlock = pullableBlock;
            _pullingTimer = new Timer(RequiredTimePulling);
        }

        private void StartCheckPulling()
        {
            _checkPulling = true;
            
            _pullingTimer.SetDuration(RequiredTimePulling);
            _pullingTimer.Clear();
        }
        private void StopCheckPulling()
        {
            _checkPulling = false;
        }

        private void UpdateCheckPulling()
        {
            if (CanPull())
            {
                if (_pullingTimer.HasFinished())
                {
                    _pullingTimer.Clear();
                    Pull();
                }
                else
                {
                    _pullingTimer.Update(Time.deltaTime);   
                }
            }
        }

        private bool CanPull()
        {
            return _checkPulling &&
                   !_pullableBlock.IsMoving &&
                   DistanceRequirementIsMet() &&
                   AngleRequirementIsMet();
        }

        private bool DistanceRequirementIsMet()
        {
            float distancePlayerAnchorSnapTarget =
                Vector3.Distance(_anchorSnapTarget.Position, _anchorSnapTarget.GetUserPosition());

            return distancePlayerAnchorSnapTarget > RequiredDistanceToPull;
        }

        private bool AngleRequirementIsMet()
        {
            Vector3 toUserDirection = (_anchorSnapTarget.GetUserPosition() - _anchorSnapTarget.Position).normalized;

            float dot = Vector3.Dot(toUserDirection, _anchorSnapTarget.GetLookDirection());

            return dot > RequiredDotForPulling;
        }
        
        private void Pull()
        {
            _pullableBlock.TryPullTowardsDirection(_pullDirection);
        }
        
    }
}