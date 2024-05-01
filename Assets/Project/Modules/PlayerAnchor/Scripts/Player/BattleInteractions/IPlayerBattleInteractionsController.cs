namespace Popeye.Modules.PlayerAnchor.Player.BattleInteractions
{
    public interface IPlayerBattleInteractionsController
    {
        public struct OnBattleStarted { }

        public struct OnBattleFinished { }


        public void StartListening();
        public void StopListening();

    }
}