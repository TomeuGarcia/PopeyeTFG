using Popeye.Modules.CombatSystem;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorDamageDealerListener
    {
        void OnDamageDealt(DamageHitResult damageHitResult);
    }
}