
namespace Popeye.Modules.GameDataEvents
{
    public struct OnEnemyWaveStartEvent
    {
        public int PlayerCurrentHealth { get; private set; }

        public OnEnemyWaveStartEvent(int playerCurrentHealth)
        {
            PlayerCurrentHealth = playerCurrentHealth;
        }
    }
    
    public class EnemyWaveStartEventData
    {
        public const string NAME = "Enemy Wave Start";
        public GenericEventData GenericEventData { get; private set; }
        public int PlayerCurrentHealth { get; private set; }

        public EnemyWaveStartEventData(GenericEventData genericEventData, OnEnemyWaveStartEvent eventInfo)
        {
            GenericEventData = genericEventData;
            PlayerCurrentHealth = eventInfo.PlayerCurrentHealth;
        }
    }
}