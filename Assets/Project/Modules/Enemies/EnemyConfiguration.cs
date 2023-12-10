using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.Enemies;
using UnityEngine;

namespace Popeye.Modules.Enemies
{
    [CreateAssetMenu(menuName = "Custom/EnemyConfiguration")]
    public class EnemyConfiguration : ScriptableObject
    {
        [SerializeField] private AEnemy[] _enemies;
        private Dictionary<string, AEnemy> _idToEnemy;


        public void Init()
        {
            _idToEnemy = new Dictionary<string, AEnemy>();
            foreach (var enemy in _enemies)
            {
                _idToEnemy.Add(enemy.Id, enemy);
            }
        }

        public AEnemy GetEnemyPrefabById(string id)
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
