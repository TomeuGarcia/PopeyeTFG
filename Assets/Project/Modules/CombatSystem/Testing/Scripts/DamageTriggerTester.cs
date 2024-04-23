using System;
using Popeye.Core.Services.ServiceLocator;
using UnityEngine;

namespace Popeye.Modules.CombatSystem.Testing.Scripts
{
    public class DamageTriggerTester : MonoBehaviour
    {
        [SerializeField] private DamageHitConfig _damageHitConfig;
        private DamageTrigger[] _damageTriggers;

        private void Start()
        {
            ICombatManager combatManager = ServiceLocator.Instance.GetService<ICombatManager>();

            DamageHit damageHit = new DamageHit(_damageHitConfig);

            _damageTriggers = new DamageTrigger[transform.childCount];
            for (int i = 0; i < transform.childCount; ++i)
            {
                _damageTriggers[i] = transform.GetChild(i).GetComponent<DamageTrigger>();
                _damageTriggers[i].Configure(combatManager, damageHit);
                _damageTriggers[i].Activate();
            }
            
        }
    }
}