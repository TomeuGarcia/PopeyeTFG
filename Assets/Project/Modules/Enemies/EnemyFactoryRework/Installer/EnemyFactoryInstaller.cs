using System.Collections.Generic;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Enemies.General;
using Popeye.Modules.Enemies.Hazards;
using UnityEngine;

namespace Popeye.Modules.Enemies.EnemyFactories
{
    public class EnemyFactoryInstaller: MonoBehaviour
    {
        [SerializeField] private EnemyFactoryInstallerConfiguration _installerConfiguration;
        [SerializeField] private SpecificCaseEnemyHinterFactoryConfig _enemyHinterFactoryConfig;
        

        public void Install(ServiceLocator serviceLocator)
        {
            var hazardsFactory = serviceLocator.GetService<IHazardFactory>();
            Dictionary<EnemyID, EnemyFactoryInstallerConfiguration.EnemyMindPrefabSpawnData> enemyIdToPrefab 
                = _installerConfiguration.GetEnemyToPrefabDictionary();
            
            Dictionary<EnemyID, IEnemyMindFactoryCreator> enemyIdToMindFactory 
                = new Dictionary<EnemyID, IEnemyMindFactoryCreator>(enemyIdToPrefab.Count);
            
            
            // Setup generic enemies
            foreach (var factoryData in enemyIdToPrefab)
            {
                enemyIdToMindFactory[factoryData.Key] = 
                    new GenericEnemyMindFactoryCreator(factoryData.Value.Prefab, transform, factoryData.Value.NumberOfInitialObjects,hazardsFactory);
            }
            
            // Setup Slime
            SlimeFactoryConfiguration slimeFactoryConfiguration = _installerConfiguration.SlimeFactoryConfiguration;
            SlimeMindFactoryCreator slimeMindFactoryCreator = new SlimeMindFactoryCreator(slimeFactoryConfiguration, transform,hazardsFactory);
            EnemyID[] slimeEnemyIDs = slimeFactoryConfiguration.GetSlimeEnemyIDs();
            foreach (var slimeEnemyID in slimeEnemyIDs)
            {
                enemyIdToMindFactory[slimeEnemyID] = slimeMindFactoryCreator;
            }


            MindCreatorsEnemyFactory mindCreatorsEnemyFactory = new MindCreatorsEnemyFactory(enemyIdToMindFactory);
            serviceLocator.RegisterService<IEnemyFactory>(mindCreatorsEnemyFactory);


            SpecificCaseEnemyHinterFactory enemyHinterFactory =
                new SpecificCaseEnemyHinterFactory(_enemyHinterFactoryConfig, transform);
            serviceLocator.RegisterService<IEnemyHinterFactory>(enemyHinterFactory);
        }

        public void Uninstall(ServiceLocator serviceLocator)
        {
            serviceLocator.RemoveService<IEnemyHinterFactory>();
            serviceLocator.RemoveService<IEnemyFactory>();
        }
    }
}