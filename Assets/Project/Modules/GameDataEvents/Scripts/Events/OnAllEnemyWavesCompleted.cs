
namespace Popeye.Modules.GameDataEvents
{
    public struct OnAllEnemyWavesCompletedEvent
    {

    }
    
    public class AllEnemyWavesCompletedEventData
    {
        public const string NAME = "All Enemy Waves Completed";
        public GenericEventData GenericEventData { get; private set; }

        public AllEnemyWavesCompletedEventData(GenericEventData genericEventData, OnAllEnemyWavesCompletedEvent eventInfo)
        {
            GenericEventData = genericEventData;
        }
    }
}