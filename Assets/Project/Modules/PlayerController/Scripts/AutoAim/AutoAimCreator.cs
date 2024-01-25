using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimCreator : MonoBehaviour
    {
        [SerializeField] private AutoAimControllerConfig _autoAimControllerConfig;
        [SerializeField] private AutoAimTargetFinderConfig _autoAimTargetFinderConfig;
        [SerializeField] private AutoAimTargetFilterConfig _autoAimTargetFilterConfig;
        [SerializeField] private AutoAimTargetResultFiltererConfig autoAimTargetResultFilterConfig;
        [SerializeField] private CollisionProbingConfig _autoAimCollisionProbingConfig;
        
        
        public AutoAimController Create(Transform targeter)
        {
            AutoAimController autoAimController = new AutoAimController();

            AutoAimTargetsController autoAimTargetsController = new AutoAimTargetsController();
            AutoAimTargetFinder_PhysicsCast autoAimTargetFinder = new AutoAimTargetFinder_PhysicsCast();
            AutoAimTargetFilterer autoAimTargetFilterer = new AutoAimTargetFilterer();
            AutoAimTargetResultsFilterer autoAimTargetResultsFilterer = new AutoAimTargetResultsFilterer();

            AutoAimTargetToResultConverter autoAimTargetToResultConverter = new AutoAimTargetToResultConverter();

            autoAimController.Configure(_autoAimControllerConfig, autoAimTargetsController);
            autoAimTargetsController.Configure(autoAimTargetFinder, autoAimTargetToResultConverter,
                autoAimTargetResultsFilterer, targeter);
            autoAimTargetResultsFilterer.Configure(autoAimTargetResultFilterConfig);
            autoAimTargetFinder.Configure(_autoAimTargetFinderConfig, _autoAimCollisionProbingConfig, 
                autoAimTargetFilterer, targeter);
            autoAimTargetFilterer.Configure(_autoAimTargetFilterConfig, _autoAimCollisionProbingConfig);
            autoAimTargetToResultConverter.Configure(targeter);

            return autoAimController;
        }
        
        
    }
}