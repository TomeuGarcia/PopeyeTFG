
namespace Popeye.Modules.GameDataEvents
{
    public struct OnAllEnemyWavesCompletedEvent
    {

    }
    
    public class AllEnemyWavesCompletedEventData
    {
        public const string NAME = "Enemy Wave End";
        public GenericEventData GenericEventData { get; private set; }

        public AllEnemyWavesCompletedEventData(GenericEventData genericEventData, OnAllEnemyWavesCompletedEvent eventInfo)
        {
            GenericEventData = genericEventData;
        }
    }
}