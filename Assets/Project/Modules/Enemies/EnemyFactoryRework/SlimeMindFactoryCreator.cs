using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.Modules.Enemies.Components;
using Popeye.Modules.Enemies.General;
using Popeye.Modules.Enemies.Hazards;
using Popeye.Modules.Enemies.Slime;
using UnityEngine;

namespace Popeye.Modules.Enemies.EnemyFactories
{
    public class SlimeMindFactoryCreator: IEnemyMindFactoryCreator
    {
        private readonly IHazardFactory _hazardFactory;
        private readonly ObjectPool _slimeMindPool;
        private readonly Dictionary<EnemyID, SlimeSizeID> _slimeTypeToSize;
        private readonly SlimeFactory _slimeFactory;

        public SlimeMindFactoryCreator(SlimeFactoryConfiguration slimeFactoryConfiguration, Transform parent,IHazardFactory hazardFactory)
        {
            _hazardFactory = hazardFactory;
            Dictionary<SlimeSizeID, ObjectPool> slimeSizeToPool; 
            Dictionary<SlimeSizeID, SlimeFactory.SlimeChildSpawnData> slimeSizeToNextSize;
            
            slimeFactoryConfiguration.SetupSlimeDictionaries(parent, 
                out slimeSizeToPool, out slimeSizeToNextSize, out _slimeTypeToSize);
            
            
            _slimeFactory = new SlimeFactory(slimeSizeToPool, slimeSizeToNextSize);
            
            _slimeMindPool = new ObjectPool(slimeFactoryConfiguration.SlimeMindPrefab, parent);
            _slimeMindPool.Init(slimeFactoryConfiguration.NumberOfInitialSlimes);
        }

        public AEnemy Create(EnemyID enemyID, Vector3 position, Quaternion rotation)
        {
            SlimeMindEnemy slimeMindEnemy = _slimeMindPool.Spawn<SlimeMindEnemy>(position, rotation);
            slimeMindEnemy.SetHazardFactory(_hazardFactory);
            SlimeMediator slimeMediator =  _slimeFactory.CreateNew(_slimeTypeToSize[enemyID], slimeMindEnemy, position, rotation);
            slimeMindEnemy.InitAfterSpawn(slimeMediator);
            return slimeMindEnemy;
        }
    }
}