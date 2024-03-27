namespace Popeye.Modules.GameDataEvents
{
    public interface IGameDataEventsConsumer
    {
        void AddEnemySeesPlayerEvent(EnemySeesPlayerEventData eventData);
    }
}