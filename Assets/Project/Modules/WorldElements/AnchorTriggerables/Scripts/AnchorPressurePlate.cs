using System;
using DG.Tweening;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.WorldElements.WorldInteractors;
using Project.Scripts.TweenExtensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.WorldElements.AnchorTriggerables
{
    public class AnchorPressurePlate : MonoBehaviour, IDamageHitTarget
    {
        [Header("MOVE")] 
        [SerializeField] private TweenConfigAsset _triggeredMoveBy;

        [Header("REFERENCES")]
        [SerializeField] private Material _triggeredMaterial;
        [SerializeField] private Material _notTriggeredMaterial;
        [SerializeField] private MeshRenderer _buttonMesh;
        [SerializeField] private Transform _buttonTransform;
        
        [Header("AUDIO")]
        [SerializeField] private AFMODAudioManagerReference _audioManager;
        [SerializeField] private OneShotFMODSound _activatedSound;
    
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
            PlayTriggerAnimation();
            _isTriggered = true;
    
            ActivateWorldInteractors();
        }
    
    
        protected void PlayTriggerAnimation()
        {
            _buttonMesh.material = _triggeredMaterial;

            _buttonTransform.BlendableLocalMoveBy(_triggeredMoveBy.Config);
            
            _audioManager.PlayOneShotAttached(_activatedSound, gameObject);
        }
        protected void PlayUntriggerAnimation()
        {
            _buttonMesh.material = _notTriggeredMaterial;
            _buttonTransform.BlendableLocalMoveBy(_triggeredMoveBy.Config.Undo());
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


