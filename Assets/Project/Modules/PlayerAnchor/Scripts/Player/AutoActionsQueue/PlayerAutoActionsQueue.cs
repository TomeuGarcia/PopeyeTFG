using Popeye.Modules.PlayerAnchor.Anchor;

namespace Popeye.Modules.PlayerAnchor.Player.AutoActionsQueue
{
    public class PlayerAutoActionsQueue : IPlayerAutoActionsQueue
    {
        private readonly IPlayerMediator _player;
        private readonly IAnchorMediator _anchor;
        

        public PlayerAutoActionsQueue(IPlayerMediator player, IAnchorMediator anchor)
        {
            _player = player;
            _anchor = anchor;
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
    }
}