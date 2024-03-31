using System;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.WorldElements.WorldInteractors;
using Popeye.Scripts.EventChannels;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public class PlayerAbilityUnlocker : MonoBehaviour, IHealthBehaviourListener
    {
        [Header("TRIGGER")]
        [SerializeField] private Collider _playerCollider;
        [SerializeField] private Collider _activationTrigger;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private HealthBehaviour _healthBehaviour;
        [SerializeField] private DamageHitTargetType _hitTargetType;

        [Header("VIEW")] 
        [SerializeField] private InterfaceReference<IPlayerAbilityUnlockerView, MonoBehaviour> _view;

        [Header("WORLD INTERACTORS")]
        [SerializeField] private AWorldInteractor[] _worldInteractors;
        
        private IEmptyEventChannelDispatcher _abilityToUnlockEventChannel;
        
        
        public void Configure(IEmptyEventChannelDispatcher abilityToUnlockEventChannel)
        {
            _abilityToUnlockEventChannel = abilityToUnlockEventChannel;
            
            _healthBehaviour.Configure(this, 1, _hitTargetType, _rigidbody);
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
            _activationTrigger.isTrigger = true;
            
            _view.Value.PlayIdleAnimation();
        }


        private async UniTaskVoid UnlockAbility()
        {
            await _view.Value.PlayUnlockAbilityAnimation();            
            _abilityToUnlockEventChannel.RaiseEvent();
            DisablePlayerCollider();
            ActivateWorldInteractors();
        }

        
        public void OnHealed() { }
        public void OnDamageTaken(DamageHitResult damageHitResult) { }
        public void OnKilledByDamageTaken(DamageHitResult damageHitResult)
        {
            UnlockAbility().Forget();
        }

        private void DisablePlayerCollider()
        {
            _playerCollider.gameObject.SetActive(false);
        }

        private void ActivateWorldInteractors()
        {
            foreach (AWorldInteractor worldInteractor in _worldInteractors)
            {
                worldInteractor.AddActivationInput();
            }
        }
    }
}