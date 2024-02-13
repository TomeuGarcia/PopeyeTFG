using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.Modules.Enemies.General;
using Popeye.Modules.Enemies.Slime;
using UnityEngine;
namespace Popeye.Modules.Enemies.EnemyFactories
{
    public class SlimeMindFactoryCreator: IEnemyMindFactoryCreator
    {
        private readonly ObjectPool _slimeMindPool;
        private Dictionary<EnemyID, SlimeSizeID> _slimeTypeToSize;
        private SlimeFactory _slimeFactory;

        public SlimeMindFactoryCreator(SlimeFactory slimeFactory,SlimeMindEnemy slimeMindEnemyPrefab,Transform parent, int numberOfInitialSlimes)
        {
            _slimeFactory = slimeFactory;
            _slimeMindPool = new ObjectPool(slimeMindEnemyPrefab, parent);
            _slimeMindPool.Init(numberOfInitialSlimes);
        }

        public AEnemy Create(EnemyID enemyID, Vector3 position, Quaternion rotation)
        {
            SlimeMindEnemy slimeMindEnemy = _slimeMindPool.Spawn<SlimeMindEnemy>(position, rotation);
            slimeMindEnemy.InitAfterSpawn();
            SlimeMediator slimeMediator =  _slimeFactory.CreateNew(_slimeTypeToSize[enemyID], slimeMindEnemy, position, rotation);
            slimeMediator.SetSlimeMind(slimeMindEnemy);
            return slimeMindEnemy;
        }
    }
}