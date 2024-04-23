using Popeye.Modules.PlayerController.AutoAim;
using UnityEngine;

namespace Project.Modules.PlayerController.Testing.AutoAim.Scripts
{
    public class AutoAimCreator_Test : MonoBehaviour
    {
        [SerializeField] private AutoAimControllerGeneralConfig _autoAimControllerGeneralConfig;

        public AutoAimController Create(Transform targeter, Transform autoAimTargetsParent)
        {
            AutoAimController autoAimController = new AutoAimController();

            AutoAimTargetingController autoAimTargetingController = new AutoAimTargetingController();
            AutoAimTargetFinder_DirectReferences autoAimTargetFinder = new AutoAimTargetFinder_DirectReferences();
            AutoAimTargetFilterer autoAimTargetFilterer = new AutoAimTargetFilterer();
            AutoAimTargetResultsFilterer autoAimTargetResultsFilterer = new AutoAimTargetResultsFilterer();

            AutoAimTargetToResultConverter autoAimTargetToResultConverter = new AutoAimTargetToResultConverter();

            autoAimController.Configure(_autoAimControllerGeneralConfig.FunctionConfig, 
                autoAimTargetingController);
            autoAimTargetingController.Configure(autoAimTargetFinder, autoAimTargetToResultConverter,
                autoAimTargetResultsFilterer, targeter);
            autoAimTargetResultsFilterer.Configure(_autoAimControllerGeneralConfig.TargetResultFiltererConfig);
            autoAimTargetFilterer.Configure(_autoAimControllerGeneralConfig.TargetFilterConfig, 
                _autoAimControllerGeneralConfig.CollisionProbingConfig);
            autoAimTargetToResultConverter.Configure(targeter);
            autoAimTargetFinder.Configure(autoAimTargetsParent);

            return autoAimController;
        }
        
        public void SetAutoAimControllerGeneralConfig(AutoAimControllerGeneralConfig config)
        {
            _autoAimControllerGeneralConfig = config;
        }
    }
}