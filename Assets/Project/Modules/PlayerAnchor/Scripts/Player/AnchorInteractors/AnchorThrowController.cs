using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Anchor;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class AnchorThrowController
    {
        private readonly IPlayerMediator _player;
        private readonly PopeyeAnchor _anchor;
        private readonly AnchorTrajectorySnapController _anchorTrajectorySnapController;

        public bool AnchorIsBeingThrown { get; private set; }

        public AnchorThrowController(
            IPlayerMediator player,
            PopeyeAnchor anchor,
            AnchorTrajectorySnapController anchorTrajectorySnapController
        )
        {
            _player = player;
            _anchor = anchor;
            _anchorTrajectorySnapController = anchorTrajectorySnapController;

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
            
            if (_anchorTrajectorySnapController.HasAutoAimTarget)
            {
                _anchor.SetGrabbedBySnapper(_anchorTrajectorySnapController.AnchorSnapTarget);
                _anchorTrajectorySnapController.ClearState();
                return;
            }
            
            _anchor.SetRestingOnFloor();
        }
    }
}