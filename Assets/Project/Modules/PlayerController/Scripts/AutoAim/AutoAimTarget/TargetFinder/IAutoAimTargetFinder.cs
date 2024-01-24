namespace Popeye.Modules.PlayerController.AutoAim
{
    public interface IAutoAimTargetFinder
    {
        bool GetAutoAimTargets(out IAutoAimTarget[] targets);
    }
}