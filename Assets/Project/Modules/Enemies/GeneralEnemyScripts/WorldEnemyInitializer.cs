using System;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Enemies;
using UnityEngine;

namespace Popeye.Modules.Enemies.GeneralEnemyScripts
{
    public class WorldEnemyInitializer : MonoBehaviour
    {
        [SerializeField] private AEnemy _enemyPrefab;
        private AEnemy _spawnedEnemy;

        private void Start()
        {
            _spawnedEnemy = Instantiate(_enemyPrefab, transform);
            
            _spawnedEnemy.SetAttackTarget(ServiceLocator.Instance.GetService<IGameReferences>().GetPlayer());
        }
    }
}