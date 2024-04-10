namespace Popeye.Modules.PlayerAnchor.Player.AutoActionsQueue
{
    public interface IPlayerAutoActionsQueue
    {
        bool TryQueueAnchorPull();
        void ProtectPlayerWhenSceneLoading();
    }
}