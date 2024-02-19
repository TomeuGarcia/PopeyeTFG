namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public enum PlayerStates
    {
        None,
        
        Spawning,
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
        
        Healing,
        FallingOnVoid
    }
}