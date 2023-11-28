using Popeye.Modules.PlayerController.Inputs;
using Project.Modules.PlayerAnchor.Anchor;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class PlayerStatesBlackboard
    {
        public IPlayerMediator PlayerMediator { get; }
        public PlayerAnchorMovesetInputsController MovesetInputsController { get; }
        public IAnchorMediator AnchorMediator { get; }


        public PlayerStatesBlackboard(IPlayerMediator playerMediator, 
                                        PlayerAnchorMovesetInputsController movesetInputsController,
                                        IAnchorMediator anchorMediator)
        {
            PlayerMediator = playerMediator;
            MovesetInputsController = movesetInputsController;
            AnchorMediator = anchorMediator;
        }
    }
}