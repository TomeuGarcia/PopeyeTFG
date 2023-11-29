using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using Popeye.Modules.PlayerController.Inputs;
using Project.Modules.PlayerAnchor.Anchor;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class PlayerStatesBlackboard
    {
        public PlayerStatesConfig PlayerStatesConfig { get; private set; }
        public IPlayerMediator PlayerMediator { get; private set;  }
        public PlayerAnchorMovesetInputsController MovesetInputsController { get; private set;  }
        public IAnchorMediator AnchorMediator { get; private set;  }
        public IAnchorThrower AnchorThrower { get; private set;  }
        


        public void Configure(PlayerStatesConfig playerStatesConfig,
                                    IPlayerMediator playerMediator, 
                                    PlayerAnchorMovesetInputsController movesetInputsController,
                                    IAnchorMediator anchorMediator,
                                    IAnchorThrower anchorThrower)
        {
            PlayerStatesConfig = playerStatesConfig;
            PlayerMediator = playerMediator;
            MovesetInputsController = movesetInputsController;
            AnchorMediator = anchorMediator;
            AnchorThrower = anchorThrower;
        }
    }
}