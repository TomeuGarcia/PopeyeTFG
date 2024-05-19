namespace Popeye.Modules.GameDataEvents
{
    public struct OnEnemyWavesSpawnerStartEvent
    {
        public string Id { get; private set; }
        public int NumberOfWaves { get; private set; }

        public OnEnemyWavesSpawnerStartEvent(string id, int numberOfWaves)
        {
            Id = id;
            NumberOfWaves = numberOfWaves;
        }
    }
    
    public class EnemyWavesSpawnerStartEventData
    {
        public const string NAME = "Enemy Waves Start Info";
        public GenericEventData GenericEventData { get; private set; }
        public string Id { get; private set; }
        public int NumberOfWaves { get; private set; }

        public EnemyWavesSpawnerStartEventData(GenericEventData genericEventData, OnEnemyWavesSpawnerStartEvent eventInfo)
        {
            GenericEventData = genericEventData;
            Id = eventInfo.Id;
            NumberOfWaves = eventInfo.NumberOfWaves;
        }
    }
}