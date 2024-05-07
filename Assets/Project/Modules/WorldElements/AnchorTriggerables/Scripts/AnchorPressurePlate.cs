using Popeye.Modules.CombatSystem;
using Popeye.Modules.WorldElements.WorldInteractors;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.WorldElements.AnchorTriggerables
{
    public class AnchorPressurePlate : MonoBehaviour, IDamageHitTarget
    {
        [Header("AUDIO")]
        [SerializeField] private ButtonInteractorAudio _audio;
        
        [Header("REFERENCES")]
        [SerializeField] private AnchorButtonView _view;
    
        [Header("WORLD INTERACTORS")]
        [SerializeField] private AWorldInteractor[] _worldInteractors;
    
        protected bool _isTriggered;

        private Vector3 Position => transform.position;

    
    
        private void Awake()
        {
            _isTriggered = false;
        }
        
        public bool CanBeDamaged(DamageHit damageHit)
        {
            return CanBeTriggered();
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
    
            return new DamageHitResult(this, gameObject, damageHit, 0, Position);
        }
    
        protected virtual bool CanBeTriggered()
        {
            return !_isTriggered;
        }
    
        protected virtual void OnTakeAnchorHit()
        {
            _isTriggered = true;
            PlayTriggerAnimation();
    
            ActivateWorldInteractors();
        }
    
    
        protected void PlayTriggerAnimation()
        {
            _view.PlayTriggeredAnimation();
            _audio.PlayActivatedSound(gameObject);
        }
        protected void PlayUntriggerAnimation()
        {
            _view.PlayNotTriggeredAnimation();
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


