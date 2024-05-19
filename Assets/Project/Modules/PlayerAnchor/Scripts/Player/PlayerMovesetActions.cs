using System;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public enum PlayerMovesetActions
    {
        AnchorThrow, 
        AnchorPull,
        DashToAnchor,
        DashDropAnchor,
        DashDropSlam,
        AnchorSpin,
        ChainSpikes,
        Rage
    }

    public static class PlayerMovesetActionsHelper
    {
        public static PlayerMovesetActions[] GetAllValuesArray()
        {
            return Enum.GetValues(typeof(PlayerMovesetActions)) as PlayerMovesetActions[];
        }
    }
}