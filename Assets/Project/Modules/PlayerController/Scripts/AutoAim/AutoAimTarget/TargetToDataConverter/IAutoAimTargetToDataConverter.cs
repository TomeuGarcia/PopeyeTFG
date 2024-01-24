namespace Popeye.Modules.PlayerController.AutoAim
{
    public interface IAutoAimTargetToDataConverter
    {
        AutoAimTargetData[] Convert(IAutoAimTarget[] autoAimTargets);
    }
}