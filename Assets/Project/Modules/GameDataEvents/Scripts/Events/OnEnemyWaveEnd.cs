
namespace Popeye.Modules.GameDataEvents
{
    public struct OnEnemyWaveEndEvent
    {
        public int PlayerCurrentHealth { get; private set; }

        public OnEnemyWaveEndEvent(int playerCurrentHealth)
        {
            PlayerCurrentHealth = playerCurrentHealth;
        }
    }
    
    public class EnemyWaveEndEventData
    {
        public const string NAME = "Enemy Wave End";
        public GenericEventData GenericEventData { get; private set; }
        public int PlayerCurrentHealth { get; private set; }

        public EnemyWaveEndEventData(GenericEventData genericEventData, OnEnemyWaveEndEvent eventInfo)
        {
            GenericEventData = genericEventData;
            PlayerCurrentHealth = eventInfo.PlayerCurrentHealth;
        }
    }
}