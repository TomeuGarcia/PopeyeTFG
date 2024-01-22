using UnityEngine;

namespace Project.Modules.CombatSystem.KnockbackSystem
{
    public interface IKnockbackManager
    {
        bool TryApplyKnockback(GameObject hitObject, KnockbackHit knockbackHit);
    }
}