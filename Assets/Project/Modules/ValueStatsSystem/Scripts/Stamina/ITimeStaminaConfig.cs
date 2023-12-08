namespace Popeye.Modules.ValueStatSystem
{
    public interface ITimeStaminaConfig : IStaminaConfig
    {
        public float FullRecoverDuration { get; }
        public float RecoverAfterUseDelayDuration { get; }
        public float RecoverAfterExhaustDelayDuration { get; }
    }
}