using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using Popeye.Modules.PlayerController.AutoAim;
using UnityEngine;

namespace Project.Modules.PlayerController.Testing.AutoAim.Scripts
{
    public class AutoAimCreator_Test : MonoBehaviour
    {
        [SerializeField] private AutoAimControllerConfig _autoAimControllerConfig;
        [SerializeField] private AutoAimTargetFilterConfig _autoAimTargetFilterConfig;
        [SerializeField] private CollisionProbingConfig _autoAimCollisionProbingConfig;



        public AutoAimController Create(Transform targeter, Vector3 startForwardDirection, Vector3 startRightDirection,
            Transform autoAimTargetsParent)
        {
            AutoAimController autoAimController = new AutoAimController();

            AutoAimTargetsController autoAimTargetsController = new AutoAimTargetsController();
            AutoAimTargetFinder_DirectReferences autoAimTargetFinder = new AutoAimTargetFinder_DirectReferences();
            AutoAimTargetFilterer autoAimTargetFilterer = new AutoAimTargetFilterer();

            AutoAimTargetToDataConverter autoAimTargetToDataConverter = new AutoAimTargetToDataConverter();

            autoAimController.Configure(_autoAimControllerConfig, autoAimTargetsController);
            autoAimTargetsController.Configure(autoAimTargetFinder, autoAimTargetToDataConverter);
            autoAimTargetFilterer.Configure(_autoAimTargetFilterConfig, _autoAimCollisionProbingConfig);
            autoAimTargetToDataConverter.Configure(targeter, startForwardDirection, startRightDirection);
            autoAimTargetFinder.Configure(autoAimTargetsParent);

            return autoAimController;
        }
    }
}