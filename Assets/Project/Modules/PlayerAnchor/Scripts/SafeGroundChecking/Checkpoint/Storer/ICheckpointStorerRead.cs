namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.Checkpoint
{
    public interface ICheckpointStorerRead
    {
        ICheckpointData LastSafeCheckpoint { get; } 
    }
}