
namespace Popeye.Modules.GameDataEvents
{
    public struct OnAllEnemyWavesCompletedEvent
    {
        public int PlayerCurrentHealth { get; private set; }

        public OnAllEnemyWavesCompletedEvent(int playerCurrentHealth)
        {
            PlayerCurrentHealth = playerCurrentHealth;
        }
    }
    
    public class AllEnemyWavesCompletedEventData
    {
        public const string NAME = "Enemy Wave End";
        public GenericEventData GenericEventData { get; private set; }
        public int PlayerCurrentHealth { get; private set; }

        public AllEnemyWavesCompletedEventData(GenericEventData genericEventData, OnAllEnemyWavesCompletedEvent eventInfo)
        {
            GenericEventData = genericEventData;
            PlayerCurrentHealth = eventInfo.PlayerCurrentHealth;
        }
    }
}