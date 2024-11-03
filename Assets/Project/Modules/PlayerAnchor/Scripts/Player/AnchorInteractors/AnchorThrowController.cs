using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Anchor;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class AnchorThrowController
    {
        private readonly IPlayerMediator _player;
        private readonly PopeyeAnchor _anchor;
        private readonly AnchorTrajectorySnapController _trajectorySnapController;

        public bool AnchorIsBeingThrown { get; private set; }

        public AnchorThrowController(
            IPlayerMediator player,
            PopeyeAnchor anchor,
            AnchorTrajectorySnapController anchorTrajectorySnapController
        )
        {
            _player = player;
            _anchor = anchor;
            _trajectorySnapController = anchorTrajectorySnapController;

            AnchorIsBeingThrown = false;
        }


        public async UniTaskVoid DoThrowAnchor(AnchorThrowResult anchorThrowResult)
        {
            AnchorIsBeingThrown = true;
            await UniTask.Delay(TimeSpan.FromSeconds(anchorThrowResult.Duration));
            AnchorIsBeingThrown = false;

            OnThrowCompleted(anchorThrowResult);
        }

        private void OnThrowCompleted(AnchorThrowResult anchorThrowResult)
        {
            if (anchorThrowResult.EndsOnVoid)
            {
                _player.OnAnchorEndedInVoid();
                return;
            }
            
            if (_trajectorySnapController.HasAutoAimTarget)
            {
                _anchor.SetGrabbedBySnapper(_trajectorySnapController.AnchorSnapTarget);
                _trajectorySnapController.ClearState();
                return;
            }
            
            _anchor.SetRestingOnFloor();
        }
    }
}