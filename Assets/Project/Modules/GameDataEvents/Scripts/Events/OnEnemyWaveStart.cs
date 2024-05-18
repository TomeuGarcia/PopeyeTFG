
using System.Collections.Generic;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Enemies.General;

namespace Popeye.Modules.GameDataEvents
{
    public struct OnEnemyWaveStartEvent
    {
        public string Id { get; private set; }
        public EnemySpawner.EnemyWave EnemyWave { get; private set; }

        public OnEnemyWaveStartEvent(string id, EnemySpawner.EnemyWave enemyWave)
        {
            Id = id;
            EnemyWave = enemyWave;
        }
    }
    
    public class EnemyWaveStartEventData
    {
        public const string NAME = "Enemy Wave Start";
        public GenericEventData GenericEventData { get; private set; }
        public string Id { get; private set; }
        public Dictionary<EnemyID, int> EnemyIdsToQuantities { get; private set; }

        public EnemyWaveStartEventData(GenericEventData genericEventData, OnEnemyWaveStartEvent eventInfo)
        {
            GenericEventData = genericEventData;
            Id = eventInfo.Id;
            
            
            IEnemyIDsCollectionService enemyIDsCollectionService = 
                ServiceLocator.Instance.GetService<IEnemyIDsCollectionService>();
            EnemyID[] allEnemyIds = enemyIDsCollectionService.EnemyIDs;
            
            
            EnemyIdsToQuantities = new Dictionary<EnemyID, int>(allEnemyIds.Length);
            foreach (EnemyID enemyId in allEnemyIds)
            {
                EnemyIdsToQuantities.Add(enemyId, 0);
            }
            
            EnemySpawner.EnemyWave.SpawnSequenceBeat[] spawnSequence = eventInfo.EnemyWave.SpawnSequence;
            for (int i = 0; i < eventInfo.EnemyWave.NumberOfEnemies; ++i)
            {
                EnemyID enemyID = spawnSequence[i].EnemyID;
                EnemyIdsToQuantities[enemyID] += 1;
            }
        }
    }
}