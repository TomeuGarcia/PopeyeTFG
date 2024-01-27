using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using Popeye.Modules.PlayerController.AutoAim;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Modules.PlayerController.Testing.AutoAim.Scripts
{
    public class AutoAimCreator_Test : MonoBehaviour
    {
        [SerializeField] private AutoAimControllerGeneralConfig _autoAimControllerGeneralConfig;

        public AutoAimController Create(Transform targeter, Transform autoAimTargetsParent)
        {
            AutoAimController autoAimController = new AutoAimController();

            AutoAimTargettingController autoAimTargettingController = new AutoAimTargettingController();
            AutoAimTargetFinder_DirectReferences autoAimTargetFinder = new AutoAimTargetFinder_DirectReferences();
            AutoAimTargetFilterer autoAimTargetFilterer = new AutoAimTargetFilterer();
            AutoAimTargetResultsFilterer autoAimTargetResultsFilterer = new AutoAimTargetResultsFilterer();

            AutoAimTargetToResultConverter autoAimTargetToResultConverter = new AutoAimTargetToResultConverter();

            autoAimController.Configure(_autoAimControllerGeneralConfig.FunctionConfig, 
                autoAimTargettingController);
            autoAimTargettingController.Configure(autoAimTargetFinder, autoAimTargetToResultConverter,
                autoAimTargetResultsFilterer, targeter);
            autoAimTargetResultsFilterer.Configure(_autoAimControllerGeneralConfig.TargetResultFiltererConfig);
            autoAimTargetFilterer.Configure(_autoAimControllerGeneralConfig.TargetFilterConfig, 
                _autoAimControllerGeneralConfig.CollisionProbingConfig);
            autoAimTargetToResultConverter.Configure(targeter);
            autoAimTargetFinder.Configure(autoAimTargetsParent);

            return autoAimController;
        }
    }
}