using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimCreator : MonoBehaviour
    {
        [SerializeField] private AutoAimControllerGeneralConfig _autoAimControllerGeneralConfig;
        
        public AutoAimController Create(Transform targeter)
        {
            AutoAimController autoAimController = new AutoAimController();

            AutoAimTargetingController autoAimTargetingController = new AutoAimTargetingController();
            AutoAimTargetFinder_PhysicsCast autoAimTargetFinder = new AutoAimTargetFinder_PhysicsCast();
            AutoAimTargetFilterer autoAimTargetFilterer = new AutoAimTargetFilterer();
            AutoAimTargetResultsFilterer autoAimTargetResultsFilterer = new AutoAimTargetResultsFilterer();

            AutoAimTargetToResultConverter autoAimTargetToResultConverter = new AutoAimTargetToResultConverter();

            
            autoAimController.Configure(_autoAimControllerGeneralConfig.FunctionConfig, 
                autoAimTargetingController);
            
            autoAimTargetingController.Configure(autoAimTargetFinder, autoAimTargetToResultConverter,
                autoAimTargetResultsFilterer, targeter);
            
            autoAimTargetResultsFilterer.Configure(_autoAimControllerGeneralConfig.TargetResultFiltererConfig);
            
            autoAimTargetFinder.Configure(_autoAimControllerGeneralConfig.TargetFinderConfig, 
                _autoAimControllerGeneralConfig.CollisionProbingConfig, autoAimTargetFilterer, targeter);
            
            autoAimTargetFilterer.Configure(_autoAimControllerGeneralConfig.TargetFilterConfig, 
                _autoAimControllerGeneralConfig.CollisionProbingConfig);
            
            autoAimTargetToResultConverter.Configure(targeter);

            return autoAimController;
        }

        public void SetAutoAimControllerGeneralConfig(AutoAimControllerGeneralConfig config)
        {
            _autoAimControllerGeneralConfig = config;
        }
    }
}