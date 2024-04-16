using System;
using UnityEngine;

namespace Popeye.Modules.CombatSystem.Testing.Scripts
{
    public abstract class ADestructiblePropDeathwish : MonoBehaviour
    {
        [Header("DESTRUCTIBLE PROP")]
        [SerializeField] private DestructibleProp _destructibleProp;

        private void Start()
        {
            _destructibleProp.HealthSystem.OnDeath += DoDeathwish;
        }
        private void OnDestroy()
        {
            _destructibleProp.HealthSystem.OnDeath -= DoDeathwish;
        }

        protected abstract void DoDeathwish();
    }
}