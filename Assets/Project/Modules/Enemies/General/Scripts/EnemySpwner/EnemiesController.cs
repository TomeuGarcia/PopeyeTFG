using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies
{
    public class EnemiesController : MonoBehaviour
    {
        [SerializeField] private bool _initEnemies = true;
        [SerializeField] private bool _enemiesRespawn = false;

        [Header("PARENTS")] [SerializeField] private GameObject BigEnemies;
        [SerializeField] private GameObject MediumEnemies;
        [SerializeField] private GameObject SmallEnemies;

        [Header("REFERENCES")] [SerializeField]
        private Transform _enemyAttackTarget;

        private SpiderEnemy[] _enemies;



        private void Awake()
        {
            if (!_initEnemies) return;

            int numberOfBigEnemies = BigEnemies.transform.childCount;
            int numberOfMediumEnemies = MediumEnemies.transform.childCount;
            int numberOfSmallEnemies = SmallEnemies.transform.childCount;

            int numberOfEnemies = numberOfBigEnemies + numberOfMediumEnemies + numberOfSmallEnemies;
            _enemies = new SpiderEnemy[numberOfEnemies];

            int enemyI = 0;
            for (int i = 0; i < numberOfBigEnemies; ++i, ++enemyI)
            {
                SpiderEnemy spiderEnemy = BigEnemies.transform.GetChild(i).GetComponent<SpiderEnemy>();
                spiderEnemy.AwakeInit_old(_enemyAttackTarget, _enemiesRespawn);
                spiderEnemy.SetRespawnPosition(spiderEnemy.Position);

                _enemies[enemyI] = spiderEnemy;
            }

            for (int i = 0; i < numberOfMediumEnemies; ++i, ++enemyI)
            {
                SpiderEnemy spiderEnemy = MediumEnemies.transform.GetChild(i).GetComponent<SpiderEnemy>();
                spiderEnemy.AwakeInit_old(_enemyAttackTarget, _enemiesRespawn);
                spiderEnemy.SetRespawnPosition(spiderEnemy.Position);

                _enemies[enemyI] = spiderEnemy;
            }

            for (int i = 0; i < numberOfSmallEnemies; ++i, ++enemyI)
            {
                SpiderEnemy spiderEnemy = SmallEnemies.transform.GetChild(i).GetComponent<SpiderEnemy>();
                spiderEnemy.AwakeInit_old(_enemyAttackTarget, _enemiesRespawn);
                spiderEnemy.SetRespawnPosition(spiderEnemy.Position);

                _enemies[enemyI] = spiderEnemy;
            }

        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                BigEnemies.SetActive(!BigEnemies.activeInHierarchy);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                MediumEnemies.SetActive(!MediumEnemies.activeInHierarchy);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SmallEnemies.SetActive(!SmallEnemies.activeInHierarchy);
            }
        }
    }
}
