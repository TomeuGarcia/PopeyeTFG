using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.WorldElements.WorldInteractors;
using UnityEngine;

namespace Popeye.Modules.WorldElements.AnchorTriggerables
{
    public class AnchorPressurePlate : MonoBehaviour, IDamageHitTarget
    {
        [Header("MOVE")] 
        [SerializeField] private Vector3 _triggerMoveBy = Vector3.zero;
        
        
        [Header("REFERENCES")]
        [SerializeField] private Material _triggeredMaterial;
        [SerializeField] private Material _notTriggeredMaterial;
        [SerializeField] private MeshRenderer _buttonMesh;
        [SerializeField] private Transform _buttonTransform;
        [SerializeField] protected BoxCollider _collider;
    
        [Header("WORLD INTERACTORS")]
        [SerializeField] private AWorldInteractor[] _worldInteractors;
    
        protected bool _isTriggered;

        private Vector3 Position => transform.position;
    
    
        private void Awake()
        {
            _buttonMesh.material = _notTriggeredMaterial;
            _isTriggered = false;
        }
    
        public bool CanBeDamaged(DamageHit damageHit)
        {
            return CanBeTriggered(damageHit);
        }
    
        public bool IsDead()
        {
            return false;
        }
    
        public DamageHitTargetType GetDamageHitTargetType()
        {
            return DamageHitTargetType.Interactable;
        }
    
        public DamageHitResult TakeHitDamage(DamageHit damageHit)
        {
            OnTakeAnchorHit();
    
            return new DamageHitResult(this, gameObject, 0, Position);
        }
    
        protected virtual bool CanBeTriggered(DamageHit damageHit)
        {
            /*
            if (!_collider.bounds.Contains(damageHit.Position))
            {
                return false;
            }
            */
    
            return !_isTriggered && damageHit.Damage > 10;
        }
    
        protected virtual void OnTakeAnchorHit()
        {
            PlayTriggerAnimation();
            _isTriggered = true;
    
            ActivateWorldInteractors();
        }
    
    
        protected void PlayTriggerAnimation()
        {
            _buttonMesh.material = _triggeredMaterial;
            _buttonTransform.DOBlendableLocalMoveBy(_triggerMoveBy, 0.2f);
        }
        protected void PlayUntriggerAnimation()
        {
            _buttonMesh.material = _notTriggeredMaterial;
            _buttonTransform.DOLocalMove(-_triggerMoveBy, 0.2f);
        }
    
    
        protected void DeactivateWorldInteractors()
        {
            foreach (AWorldInteractor worldInteractor in _worldInteractors)
            {
                worldInteractor.AddDeactivationInput();
            }
        }
    
        protected void ActivateWorldInteractors()
        {
            foreach (AWorldInteractor worldInteractor in _worldInteractors)
            {
                worldInteractor.AddActivationInput();
            }
        }
    }
}


