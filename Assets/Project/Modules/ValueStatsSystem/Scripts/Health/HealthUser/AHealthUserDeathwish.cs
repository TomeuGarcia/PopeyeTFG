using System;
using AYellowpaper;
using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    public abstract class AHealthUserDeathwish : MonoBehaviour
    {
        [Header("HEALTH USER")]
        [SerializeField] private InterfaceReference<IHealthUserBehaviour, MonoBehaviour> _healthUser;
        private IHealthUserBehaviour HealthUser => _healthUser.Value;

        private void Start()
        {
            HealthUser.HealthSystem.OnDeath += DoDeathwish;
        }
        private void OnDestroy()
        {
            HealthUser.HealthSystem.OnDeath -= DoDeathwish;
        }

        protected abstract void DoDeathwish();
    }
}