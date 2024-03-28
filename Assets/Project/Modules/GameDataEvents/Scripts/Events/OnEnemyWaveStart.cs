
namespace Popeye.Modules.GameDataEvents
{
    public struct OnEnemyWaveStartEvent
    {

    }
    
    public class EnemyWaveStartEventData
    {
        public const string NAME = "Enemy Wave Start";
        public GenericEventData GenericEventData { get; private set; }

        public EnemyWaveStartEventData(GenericEventData genericEventData, OnEnemyWaveStartEvent eventInfo)
        {
            GenericEventData = genericEventData;
        }
    }
}