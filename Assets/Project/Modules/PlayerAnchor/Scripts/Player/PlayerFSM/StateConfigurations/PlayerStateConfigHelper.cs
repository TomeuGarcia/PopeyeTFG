
namespace Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations
{
    public static class PlayerStateConfigHelper
    {
        public const string SO_ASSETS_PATH = "Popeye/PlayerAnchor/PlayerStateConfigs/";


        [System.Serializable]
        public class ConfigurationsGroup
        {
            public Spawning_PlayerStateConfig spawning;
            public Dead_PlayerStateConfig dead;
            public WithAnchor_PlayerStateConfig withAnchor;
            public WithoutAnchor_PlayerStateConfig withoutAnchor;

            public MovingWithAnchor_PlayerStateConfig movingWithAnchor;
            public AimingThrowAnchor_PlayerStateConfig aimingThrowAnchor;
            public ThrowingAnchor_PlayerStateConfig throwingAnchor;
            
            public MovingWithoutAnchor_PlayerStateConfig movingWithoutAnchor;
            public PickingUpAnchor_PlayerStateConfig pickingUpAnchor;
            public DashingTowardsAnchor_PlayerStateConfig dashingTowardsAnchor;
            public KickingAnchor_PlayerStateConfig kickingAnchor;
            public PullingAnchor_PlayerStateConfig pullingAnchor;
            public SpinningAnchor_PlayerStateConfig spinningAnchor;
        }
    }
}