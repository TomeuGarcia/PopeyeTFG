namespace Popeye.Modules.ValueStatSystem
{
    public interface ITimeStaminaConfig : IStaminaConfig
    {
        public float FullRecoverDuration { get; }
        public float RecoverDelayAfterUseDuration { get; }
        public float RecoverDelayAfterExhaustDuration { get; }
    }
}