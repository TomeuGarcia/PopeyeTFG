using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Enemies.EnemyFactories;

namespace Popeye.Modules.Enemies.General
{
    public class EnemySpawner : MonoBehaviour
    {

        [System.Serializable]
        class EnemyWave
        {
            [SerializeField, Range(0.0f, 10.0f)] private float _delayBeforeWaveSpawning = 1.0f;
            [SerializeField] private SpawnSequenceBeat[] _spawnSequence;

            public float DelayBeforeWaveSpawning => _delayBeforeWaveSpawning;
            public SpawnSequenceBeat[] SpawnSequence => _spawnSequence;
            public int NumberOfEnemies => _spawnSequence.Length;


            [System.Serializable]
            public class SpawnSequenceBeat
            {
                [SerializeField, Range(0.0f, 10.0f)] private float _delayBeforeSpawn = 0.5f;
                [SerializeField] private EnemyID _enemyID;
                [SerializeField] private Transform _spawnSpot;

                public float DelayBeforeSpawn => _delayBeforeSpawn;
                public EnemyID EnemyID => _enemyID;
                public Vector3 SpawnPosition => _spawnSpot.position;
            }

        }



        [SerializeField] private Transform _enemyAttackTarget;
        [SerializeField] private EnemyWave[] _enemyWaves;
        private int _activeEnemiesCount;
        private bool AllCurrentWaveEnemiesAreDead => _activeEnemiesCount == 0;
        public delegate void EnemySpawnerEvent();

        public EnemySpawnerEvent OnFirstWaveStarted;
        public EnemySpawnerEvent OnAllWavesFinished;
        
        private IEnemyFactory _enemyFactory;

        private void Start()
        {
            _enemyFactory = ServiceLocator.Instance.GetService<IEnemyFactory>();
        }

        public void StartWaves()
        {
            DoStartWaves().Forget();
        }


        private async UniTaskVoid DoStartWaves()
        {
            OnFirstWaveStarted?.Invoke();

            for (int waveI = 0; waveI < _enemyWaves.Length; ++waveI)
            {
                await SpawnEnemyWave(_enemyWaves[waveI]);
                await UniTask.WaitUntil(() => AllCurrentWaveEnemiesAreDead);
            }

            OnAllWavesFinished?.Invoke();
        }

        private async UniTask SpawnEnemyWave(EnemyWave enemyWave)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(enemyWave.DelayBeforeWaveSpawning));


            _activeEnemiesCount = enemyWave.NumberOfEnemies;

            for (int i = 0; i < enemyWave.SpawnSequence.Length; ++i)
            {
                EnemyWave.SpawnSequenceBeat spawnSequenceBeat = enemyWave.SpawnSequence[i];

                await UniTask.Delay(TimeSpan.FromSeconds(spawnSequenceBeat.DelayBeforeSpawn));
                SpawnEnemy(spawnSequenceBeat.EnemyID, spawnSequenceBeat.SpawnPosition);
            }
        }

        private void SpawnEnemy(EnemyID enemyID, Vector3 spawnPosition)
        {
            AEnemy enemy = _enemyFactory.Create(enemyID, spawnPosition, Quaternion.identity);
            enemy.AwakeInit(_enemyAttackTarget);

            enemy.OnDeathComplete += DecrementActiveEnemiesCount;
        }
        
        
        
        private void DecrementActiveEnemiesCount(AEnemy destroyedEnemy)
        {
            destroyedEnemy.OnDeathComplete -= DecrementActiveEnemiesCount;

            --_activeEnemiesCount;
        }

    }
}
