using System;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    public abstract class AHealthUserDeathwish : MonoBehaviour
    {
        [Header("DELAY")]
        [SerializeField, Range(0f, 10f)] private float _delay = 0f;
        
        [Header("HEALTH USER")]
        [SerializeField] private InterfaceReference<IHealthUserBehaviour, MonoBehaviour> _healthUser;
        private IHealthUserBehaviour HealthUser => _healthUser.Value;

        private void Start()
        {
            HealthUser.HealthSystem.OnDeath += StartDeathwish;
        }
        private void OnDestroy()
        {
            HealthUser.HealthSystem.OnDeath -= StartDeathwish;
        }

        private async void StartDeathwish()
        {
            if (_delay > 0.001f)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_delay));    
            }            
            DoDeathwish();
        }
        protected abstract void DoDeathwish();
    }
}