namespace Popeye.Modules.PlayerAnchor.Player.PlayerEvents
{
    public interface IPlayerEventsDispatcher
    {
        public struct OnDieEvent { }
        public struct OnRespawnFromDeathEvent { }


        void DispatchOnDiedEvent();
        void DispatchOnRespawnFromDeathEvent();


    }
}