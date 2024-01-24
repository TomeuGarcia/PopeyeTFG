using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimCreator : MonoBehaviour
    {
        [SerializeField] private AutoAimControllerConfig _autoAimControllerConfig;
        [SerializeField] private AutoAimTargetFinderConfig _autoAimTargetFinderConfig;
        [SerializeField] private AutoAimTargetFilterConfig _autoAimTargetFilterConfig;
        [SerializeField] private CollisionProbingConfig _autoAimCollisionProbingConfig;
        
        
        public AutoAimController Create(Transform targeter, Vector3 startForwardDirection, Vector3 startRightDirection)
        {
            AutoAimController autoAimController = new AutoAimController();

            AutoAimTargetsController autoAimTargetsController = new AutoAimTargetsController();
            AutoAimTargetFinder_PhysicsCast autoAimTargetFinder = new AutoAimTargetFinder_PhysicsCast();
            AutoAimTargetFilterer autoAimTargetFilterer = new AutoAimTargetFilterer();

            AutoAimTargetToDataConverter autoAimTargetToDataConverter = new AutoAimTargetToDataConverter();

            autoAimController.Configure(_autoAimControllerConfig, autoAimTargetsController);
            autoAimTargetsController.Configure(autoAimTargetFinder, autoAimTargetToDataConverter);
            autoAimTargetFinder.Configure(_autoAimTargetFinderConfig, _autoAimCollisionProbingConfig, 
                autoAimTargetFilterer, targeter);
            autoAimTargetFilterer.Configure(_autoAimTargetFilterConfig, _autoAimCollisionProbingConfig);
            autoAimTargetToDataConverter.Configure(targeter, startForwardDirection, startRightDirection);

            return autoAimController;
        }
        
        
    }
}