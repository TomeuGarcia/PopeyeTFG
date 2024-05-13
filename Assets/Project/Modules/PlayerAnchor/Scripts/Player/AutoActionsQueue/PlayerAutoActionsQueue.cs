using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Anchor;
using Popeye.Modules.PlayerController.Inputs;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.AutoActionsQueue
{
    public class PlayerAutoActionsQueue : IPlayerAutoActionsQueue
    {
        private readonly IPlayerMediator _player;
        private readonly IAnchorMediator _anchor;
        private IMovementInputEnabler _playerMovementEnabler;


        public PlayerAutoActionsQueue(IPlayerMediator player, IAnchorMediator anchor)
        {
            _player = player;
            _anchor = anchor;
        }

        public void Configure(IMovementInputEnabler playerMovementEnabler)
        {
            _playerMovementEnabler = playerMovementEnabler;
        }
        
        public bool TryQueueAnchorPull()
        {
            bool anchorCanBePulled = !_anchor.IsBeingCarried() && !_anchor.IsBeingPulled();
            
            if (anchorCanBePulled)
            {
                _player.QueuePullAnchor().Forget();
            }

            return anchorCanBePulled;
        }

        public void ProtectPlayerWhenSceneLoading()
        {
            _player.DisableSafeGroundCheckingForDuration(2.0f).Forget();
        }

        public async UniTaskVoid StopPlayerForDuration(float duration)
        {
            _playerMovementEnabler.DisableMovement();
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            _playerMovementEnabler.EnableMovement();
        }
    }
}