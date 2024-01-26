using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using Popeye.Modules.PlayerController.AutoAim;
using UnityEngine;

namespace Project.Modules.PlayerController.Testing.AutoAim.Scripts
{
    public class AutoAimCreator_Test : MonoBehaviour
    {
        [SerializeField] private AutoAimControllerConfig _autoAimControllerConfig;
        [SerializeField] private AutoAimTargetFilterConfig _autoAimTargetFilterConfig;
        [SerializeField] private AutoAimTargetResultFiltererConfig _autoAimTargetResultFiltererConfig;
        [SerializeField] private CollisionProbingConfig _autoAimCollisionProbingConfig;



        public AutoAimController Create(Transform targeter, Transform autoAimTargetsParent)
        {
            AutoAimController autoAimController = new AutoAimController();

            AutoAimTargettingController autoAimTargettingController = new AutoAimTargettingController();
            AutoAimTargetFinder_DirectReferences autoAimTargetFinder = new AutoAimTargetFinder_DirectReferences();
            AutoAimTargetFilterer autoAimTargetFilterer = new AutoAimTargetFilterer();
            AutoAimTargetResultsFilterer autoAimTargetResultsFilterer = new AutoAimTargetResultsFilterer();

            AutoAimTargetToResultConverter autoAimTargetToResultConverter = new AutoAimTargetToResultConverter();

            autoAimController.Configure(_autoAimControllerConfig, autoAimTargettingController);
            autoAimTargettingController.Configure(autoAimTargetFinder, autoAimTargetToResultConverter,
                autoAimTargetResultsFilterer, targeter);
            autoAimTargetResultsFilterer.Configure(_autoAimTargetResultFiltererConfig);
            autoAimTargetFilterer.Configure(_autoAimTargetFilterConfig, _autoAimCollisionProbingConfig);
            autoAimTargetToResultConverter.Configure(targeter);
            autoAimTargetFinder.Configure(autoAimTargetsParent);

            return autoAimController;
        }
    }
}