namespace Popeye.Modules.ValueStatSystem
{
    public class StaminaConfig : IStaminaConfig
    {
        public int MaxStamina { get; private set; }
        public int SpawnStamina { get; private set; }

        public StaminaConfig(int maxStamina, int spawnStamina)
        {
            MaxStamina = maxStamina;
            SpawnStamina = spawnStamina;
        }
    }
}