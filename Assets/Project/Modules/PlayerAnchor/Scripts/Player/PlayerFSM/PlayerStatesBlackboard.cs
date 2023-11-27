using Popeye.Modules.PlayerController.Inputs;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class PlayerStatesBlackboard
    {
        public IPlayerMediator PlayerMediator { get; }
        public PlayerAnchorMovesetInputsController MovesetInputsController { get; }


        public PlayerStatesBlackboard(IPlayerMediator playerMediator, 
                                        PlayerAnchorMovesetInputsController movesetInputsController)
        {
            PlayerMediator = playerMediator;
            MovesetInputsController = movesetInputsController;
        }
    }
}