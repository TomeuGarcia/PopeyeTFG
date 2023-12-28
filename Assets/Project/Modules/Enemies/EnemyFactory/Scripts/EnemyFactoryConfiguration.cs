using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.Enemies;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.Enemies
{
    [CreateAssetMenu(fileName = "EnemyFactoryConfiguration", 
        menuName = ScriptableObjectsHelper.ENEMIES_ASSET_PATH + "EnemyFactoryConfiguration")]
    public class EnemyFactoryConfiguration : ScriptableObject
    {
        [SerializeField] private AEnemy[] _enemyPrefabs;
        private Dictionary<Guid, AEnemy> _idToEnemy;


        public void Init()
        {
            _idToEnemy = new Dictionary<Guid, AEnemy>();
            
            foreach (var enemy in _enemyPrefabs)
            {
                _idToEnemy.Add(enemy.Id.Id, enemy);
            }
        }

        public AEnemy GetEnemyPrefabById(Guid id)
        {
            AEnemy enemy;
            if (!_idToEnemy.TryGetValue(id, out enemy))
            {
                throw new Exception($"enemy with id {id} was not found");
            }

            return enemy;
        }
    }
}
