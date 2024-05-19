
namespace Popeye.Modules.GameDataEvents
{
    public struct OnAllEnemyWavesCompletedEvent
    {
        public string Id { get; private set; }

        public OnAllEnemyWavesCompletedEvent(string id)
        {
            Id = id;
        }
    }
    
    public class AllEnemyWavesCompletedEventData
    {
        public const string NAME = "All Enemy Waves Completed";
        public GenericEventData GenericEventData { get; private set; }
        public string Id { get; private set; }

        public AllEnemyWavesCompletedEventData(GenericEventData genericEventData, OnAllEnemyWavesCompletedEvent eventInfo)
        {
            GenericEventData = genericEventData;
            Id = eventInfo.Id;
        }
    }
}