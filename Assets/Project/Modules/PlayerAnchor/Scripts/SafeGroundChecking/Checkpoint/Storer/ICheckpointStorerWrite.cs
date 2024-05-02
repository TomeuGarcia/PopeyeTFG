namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.Checkpoint
{
    public interface ICheckpointStorerWrite
    {
        void SetLastSafeCheckpoint(ICheckpointData checkpointData);
    }
}