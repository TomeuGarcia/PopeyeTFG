namespace Popeye.Modules.ValueStatSystem
{
    public interface IStaminaSystem
    {
        bool HasMaxStamina();
        bool HasStaminaLeft();
        void Spend(int spendAmount);
    }
}