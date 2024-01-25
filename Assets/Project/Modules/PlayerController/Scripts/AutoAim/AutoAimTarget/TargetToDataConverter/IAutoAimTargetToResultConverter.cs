namespace Popeye.Modules.PlayerController.AutoAim
{
    public interface IAutoAimTargetToResultConverter
    {
        AutoAimTargetResult[] Convert(IAutoAimTarget[] autoAimTargets);
        AutoAimTargetResult Convert(IAutoAimTarget autoAimTarget);
    }
}