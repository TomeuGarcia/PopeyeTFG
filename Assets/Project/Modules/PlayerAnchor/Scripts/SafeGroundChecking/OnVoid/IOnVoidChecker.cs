namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.OnVoid
{
    public interface IOnVoidChecker
    {
        bool IsOnVoid { get; }
        void UpdateChecking(float deltaTime);
        void ClearState();
    }
}