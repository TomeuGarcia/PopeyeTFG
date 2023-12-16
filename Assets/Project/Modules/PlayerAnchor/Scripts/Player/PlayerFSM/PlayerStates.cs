namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public enum PlayerStates
    {
        None,
        
        Spawning,
        WithAnchor,
        WithoutAnchor,
        Dead,
        
        MovingWithAnchor,
        AimingThrowAnchor,
        ThrowingAnchor,
        DashingDroppingAnchor,
        
        MovingWithoutAnchor,
        PickingUpAnchor,
        DashingTowardsAnchor,
        KickingAnchor,
        PullingAnchor,
        SpinningAnchor,
        Tired,
        
        Healing
    }
}