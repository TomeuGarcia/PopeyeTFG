using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimCreator : MonoBehaviour
    {
        [SerializeField] private AutoAimControllerGeneralConfig _autoAimControllerGeneralConfig;
        
        public AutoAimController Create(Transform targeter)
        {
            AutoAimController autoAimController = new AutoAimController();

            AutoAimTargettingController autoAimTargettingController = new AutoAimTargettingController();
            AutoAimTargetFinder_PhysicsCast autoAimTargetFinder = new AutoAimTargetFinder_PhysicsCast();
            AutoAimTargetFilterer autoAimTargetFilterer = new AutoAimTargetFilterer();
            AutoAimTargetResultsFilterer autoAimTargetResultsFilterer = new AutoAimTargetResultsFilterer();

            AutoAimTargetToResultConverter autoAimTargetToResultConverter = new AutoAimTargetToResultConverter();

            
            autoAimController.Configure(_autoAimControllerGeneralConfig.FunctionConfig, 
                autoAimTargettingController);
            
            autoAimTargettingController.Configure(autoAimTargetFinder, autoAimTargetToResultConverter,
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