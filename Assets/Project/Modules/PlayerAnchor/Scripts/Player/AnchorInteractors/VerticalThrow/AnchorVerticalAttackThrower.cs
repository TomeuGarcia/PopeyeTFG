using Popeye.Modules.PlayerAnchor.Anchor;
using Popeye.Scripts.EventChannels;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class AnchorVerticalAttackThrower : IAnchorVerticalThrower
    {
        private PopeyeAnchor _anchor;
        private AnchorTrajectoryMaker _anchorTrajectoryMaker;
        private AnchorThrowConfig _verticalThrowConfig;
        private AnchorThrowController _anchorThrowController;
        private IEmptyEventChannelDispatcher _eventChannelDispatcher;

        public AnchorThrowResult AnchorVerticalThrowResult { get; private set; }
        private Quaternion _verticalThrowStartRotation;
        private Quaternion _verticalThrowEndRotation;

        
        public void Configure(
            PopeyeAnchor anchor,
            AnchorTrajectoryMaker anchorTrajectoryMaker,
            AnchorThrowConfig verticalThrowConfig,
            AnchorThrowController anchorThrowController,
            IEmptyEventChannelDispatcher eventChannelDispatcher)
        {
            _anchor = anchor;
            _anchorTrajectoryMaker = anchorTrajectoryMaker;
            _verticalThrowConfig = verticalThrowConfig;
            _anchorThrowController = anchorThrowController;
            _eventChannelDispatcher = eventChannelDispatcher;
            
            AnchorVerticalThrowResult = new AnchorThrowResult(_verticalThrowConfig.MoveInterpolationCurve,
                _verticalThrowConfig.RotateInterpolationCurve);
            
            _verticalThrowStartRotation = Quaternion.LookRotation(Vector3.up, Vector3.right);
            _verticalThrowEndRotation = Quaternion.LookRotation(Vector3.down, Vector3.left);
        }
        
        public void ThrowAnchorVertically(out float duration)
        {
            float distance = _verticalThrowConfig.MaxThrowDistance;
            duration = _verticalThrowConfig.MaxThrowMoveDuration;
            
            Vector3[] throwTrajectory = _anchorTrajectoryMaker.ComputeUpAndDownTrajectory(_anchor.Position, distance,
                out RaycastHit floorHit);
            
            AnchorVerticalThrowResult.Reset(throwTrajectory, Vector3.up, 
                _verticalThrowStartRotation, _verticalThrowEndRotation, duration, false);

            AnchorThrowUtilities.CorrectEndRotationForVisibility(AnchorVerticalThrowResult, _verticalThrowConfig.EndRotationCorrection);
            
            _anchor.SetThrownVertically(AnchorVerticalThrowResult, floorHit).Forget();
            
            _anchorThrowController.DoThrowAnchor(AnchorVerticalThrowResult).Forget();
            
            _eventChannelDispatcher.RaiseEvent();
        }
    }
}