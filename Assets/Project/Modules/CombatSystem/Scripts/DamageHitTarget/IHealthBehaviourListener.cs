namespace Project.Modules.CombatSystem
{
    public interface IHealthBehaviourListener
    {
        void OnDamageTaken(DamageHitResult damageHitResult);
        void OnKilledByDamageTaken(DamageHitResult damageHitResult);
        void OnHealed();
    }
}