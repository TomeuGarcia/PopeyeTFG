using System;
using AYellowpaper;
using Popeye.Modules.CombatSystem;
using Popeye.Scripts.EventChannels;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public class PlayerAbilityUnlocker : MonoBehaviour, IHealthBehaviourListener
    {
        [Header("TRIGGER")]
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private HealthBehaviour _healthBehaviour;
        [SerializeField] private DamageHitTargetType _hitTargetType;

        [Header("VIEW")] 
        [SerializeField] private bool _testing = true;
        [SerializeField] private InterfaceReference<IPlayerAbilityUnlockerView, MonoBehaviour> _view;

        [Header("EVENT CHANNEL")] 
        [SerializeField] private InterfaceReference<IEmptyEventChannelDispatcher, ScriptableObject> _abilityToUnlockEventChannel;
        
        private void Awake()
        {
            _healthBehaviour.Configure(this, 1, _hitTargetType, _rigidbody);
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
            _collider.isTrigger = true;
            
            _view.Value.PlayIdleAnimation();
        }


        private void UnlockAbility()
        {
            _view.Value.PlayUnlockAbilityAnimation();            
            _abilityToUnlockEventChannel.Value.RaiseEvent();
        }

        
        public void OnHealed() { }
        public void OnDamageTaken(DamageHitResult damageHitResult) { }
        public void OnKilledByDamageTaken(DamageHitResult damageHitResult)
        {
            UnlockAbility();
        }

        
    }
}