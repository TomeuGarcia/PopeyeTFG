using UnityEngine;

namespace Project.Modules.CombatSystem.KnockbackSystem
{
    public interface IKnockbackHitTarget
    {
        Rigidbody GetRigidbodyToKnockback();
        bool CanBeKnockbacked();
        float GetKnockbackEffectivenessMultiplier();
    }
}