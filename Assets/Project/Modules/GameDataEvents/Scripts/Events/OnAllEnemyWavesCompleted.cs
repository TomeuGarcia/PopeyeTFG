
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
    
    public class EnemyWaveEndEventData
    {
        public const string NAME = "Enemy Wave End";
        public GenericEventData GenericEventData { get; private set; }
        public int PlayerCurrentHealth { get; private set; }

        public EnemyWaveEndEventData(GenericEventData genericEventData, OnAllEnemyWavesCompletedEvent eventInfo)
        {
            GenericEventData = genericEventData;
            PlayerCurrentHealth = eventInfo.PlayerCurrentHealth;
        }
    }
}