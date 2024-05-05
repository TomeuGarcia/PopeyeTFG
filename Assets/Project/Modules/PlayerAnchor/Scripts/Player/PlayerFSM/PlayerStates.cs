namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public enum PlayerStates
    {
        None,
        
        Spawning,
        SpawningWithAnchorOnFloor,
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
        Tired,
        TiredPickingUpAnchor,
        
        Healing,
        
        EnteringAnchorSpinAttack,
        PerformingAnchorSpinAttack,
        
        EnteringChainSpikesAttack,
        PerformingChainSpikesAttack,
        
        FallingOnVoid
    }
}