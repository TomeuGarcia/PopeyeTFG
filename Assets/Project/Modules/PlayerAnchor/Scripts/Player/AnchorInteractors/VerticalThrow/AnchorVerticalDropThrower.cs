using Popeye.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class AnchorVerticalDropThrower : IAnchorVerticalThrower
    {
        private PopeyeAnchor _anchor;
        private AnchorTrajectoryMaker _anchorTrajectoryMaker;
        private AnchorThrowConfig _verticalThrowConfig;
        private AnchorThrowController _anchorThrowController;

        public AnchorThrowResult AnchorVerticalThrowResult { get; private set; }
        private Quaternion _verticalThrowStartRotation;
        private Quaternion _verticalThrowEndRotation;
        
        
        public void Configure(
            PopeyeAnchor anchor,
            AnchorTrajectoryMaker anchorTrajectoryMaker,
            AnchorThrowConfig verticalThrowConfig,
            AnchorThrowController anchorThrowController)
        {
            _anchor = anchor;
            _anchorTrajectoryMaker = anchorTrajectoryMaker;
            _verticalThrowConfig = verticalThrowConfig;
            _anchorThrowController = anchorThrowController;
            
            AnchorVerticalThrowResult = new AnchorThrowResult(_verticalThrowConfig.MoveInterpolationCurve,
                _verticalThrowConfig.RotateInterpolationCurve);
            
            _verticalThrowStartRotation = Quaternion.LookRotation(Vector3.up, Vector3.right);
            _verticalThrowEndRotation = Quaternion.LookRotation(Vector3.down, Vector3.left);
        }

        public void ThrowAnchorVertically(out float duration)
        {
            duration = _verticalThrowConfig.MaxThrowMoveDuration;
            Vector3[] throwTrajectory =
                _anchorTrajectoryMaker.ComputeDownToFloorTrajectory(_anchor.Position, out RaycastHit floorHit);
            
            AnchorVerticalThrowResult.Reset(throwTrajectory, Vector3.down, 
                _verticalThrowStartRotation, _verticalThrowEndRotation, duration, false);

            AnchorThrowUtilities.CorrectEndRotationForVisibility(AnchorVerticalThrowResult, _verticalThrowConfig);
            
            _anchor.SetDropped(AnchorVerticalThrowResult).Forget();
            
            _anchorThrowController.DoThrowAnchor(AnchorVerticalThrowResult).Forget();
        }
        
    }
}