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
        
        MovingWithoutAnchor,
        PickingUpAnchor,
        DashingTowardsAnchor,
        KickingAnchor,
        PullingAnchor,
        SpinningAnchor,
        Tired
    }
}