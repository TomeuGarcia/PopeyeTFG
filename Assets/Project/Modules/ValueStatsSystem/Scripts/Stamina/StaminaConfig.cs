namespace Popeye.Modules.ValueStatSystem
{
    public class StaminaConfig : IStaminaConfig
    {
        public int SpawnMaxStamina { get; private set; }
        public int SpawnStamina { get; private set; }
        public int CurrentMaxStamina { get; set; }

        public StaminaConfig(int maxStamina, int spawnStamina)
        {
            SpawnMaxStamina = maxStamina;
            SpawnStamina = spawnStamina;
            CurrentMaxStamina = SpawnMaxStamina;
        }
    }
}