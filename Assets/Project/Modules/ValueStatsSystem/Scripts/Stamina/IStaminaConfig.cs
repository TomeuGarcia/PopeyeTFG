namespace Popeye.Modules.ValueStatSystem
{
    public interface IStaminaConfig
    {
        public int SpawnMaxStamina { get; }
        public int SpawnStamina { get; }
        public int CurrentMaxStamina { get; set; }
    }
}