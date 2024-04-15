namespace Popeye.Modules.Enemies.Hazards
{
    public interface IHazardDispenser
    {
        bool CanDispense();
        void StartDispensingHazard();
    }
}