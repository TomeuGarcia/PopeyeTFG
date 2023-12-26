using Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using Popeye.Modules.PlayerController.Inputs;
using Project.Modules.PlayerAnchor.Anchor;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class PlayerStatesBlackboard
    {
        public PlayerStatesConfig PlayerStatesConfig { get; private set; }
        public IPlayerMediator PlayerMediator { get; private set;  }
        public IPlayerView PlayerView { get; private set; }
        public PlayerAnchorMovesetInputsController MovesetInputsController { get; private set;  }
        public IAnchorMediator AnchorMediator { get; private set;  }

        
        // Queues
        public bool queuedDashTowardsAnchor;
        public bool queuedAnchorPull;
        public bool queuedAnchorThrow;
        
        public bool spinAttackTowardsRight;
        
        
        
        public PlayerStates cameFromState;
        
        
        

        public void Configure(PlayerStatesConfig playerStatesConfig,
            IPlayerMediator playerMediator, IPlayerView playerView,
            PlayerAnchorMovesetInputsController movesetInputsController,
            IAnchorMediator anchorMediator)
        {
            PlayerStatesConfig = playerStatesConfig;
            PlayerMediator = playerMediator;
            PlayerView = playerView;
            MovesetInputsController = movesetInputsController;
            AnchorMediator = anchorMediator;
        }
        
        
    }
}