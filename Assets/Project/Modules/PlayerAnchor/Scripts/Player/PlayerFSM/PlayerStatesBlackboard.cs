using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using Popeye.Modules.PlayerController.Inputs;
using Popeye.Modules.PlayerAnchor.Anchor;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class PlayerStatesBlackboard
    {
        public PlayerStatesConfig PlayerStatesConfig { get; private set; }
        public IPlayerMediator PlayerMediator { get; private set;  }
        public IPlayerView PlayerView { get; private set; }
        public PlayerAnchorMovesetInputsController MovesetInputsController { get; private set;  }
        public IAnchorMediator AnchorMediator { get; private set;  }
        public PlayerMovementChecker PlayerMovementChecker { get; private set;  }

        // Queues
        public bool QueuedDashTowardsAnchor { get; set; }
        public bool QueuedAnchorPull { get; set; }
        public bool QueuedAnchorAim { get; set; }

        public bool spinAttackTowardsRight;


        public PlayerStates CameFromState { get; set; }
        
        
        

        public void Configure(PlayerStatesConfig playerStatesConfig,
            IPlayerMediator playerMediator, IPlayerView playerView,
            PlayerAnchorMovesetInputsController movesetInputsController,
            IAnchorMediator anchorMediator,
            PlayerMovementChecker playerMovementChecker)
        {
            PlayerStatesConfig = playerStatesConfig;
            PlayerMediator = playerMediator;
            PlayerView = playerView;
            MovesetInputsController = movesetInputsController;
            AnchorMediator = anchorMediator;
            PlayerMovementChecker = playerMovementChecker;
        }
        
        
    }
}