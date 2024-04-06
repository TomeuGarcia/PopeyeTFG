using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.WorldElements.WorldInteractors;
using Popeye.Scripts.ObjectTypes;
using UnityEngine;

namespace Popeye.Modules.WorldElements.AnchorTriggerables
{
    public class PlayerPressurePlate : MonoBehaviour
    {
        [Header("REFERENCES")] [SerializeField]
        private Material _triggeredMaterial;

        [SerializeField] private Material _notTriggeredMaterial;
        [SerializeField] private MeshRenderer _buttonMesh;
        [SerializeField] private Transform _buttonTransform;


        [Header("WORLD INTERACTORS")] [SerializeField]
        private AWorldInteractor[] _worldInteractors;

        private int _triggeredCount;
        
        
        [Header("ACCEPT TYPES")] 
        [SerializeField] private ObjectTypeAsset[] _acceptTypes;
        

        private void Awake()
        {
            _triggeredCount = 0;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (AcceptsOtherCollider(other))
            {
                if (_triggeredCount++ > 0) return;

                SetTriggeredState();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (AcceptsOtherCollider(other))
            {
                if (--_triggeredCount > 0) return;

                SetNotTriggeredState();
            }
        }

        private bool AcceptsOtherCollider(Collider other)
        {
            if (!other.TryGetComponent(out IObjectType otherObjectType)) return false;
            return otherObjectType.IsOfAnyType(_acceptTypes);
        }


        private void SetTriggeredState()
        {
            PlayTriggerAnimation();
            ActivateWorldInteractors();
        }

        private void SetNotTriggeredState()
        {
            PlayUntriggerAnimation();
            DeactivateWorldInteractors();
        }

        protected void PlayTriggerAnimation()
        {
            _buttonMesh.material = _triggeredMaterial;
            _buttonTransform.DOLocalMove(Vector3.down * 0.05f, 0.2f);
        }

        protected void PlayUntriggerAnimation()
        {
            _buttonMesh.material = _notTriggeredMaterial;
            _buttonTransform.DOLocalMove(Vector3.up * 0.05f, 0.2f);
        }


        private void DeactivateWorldInteractors()
        {
            foreach (AWorldInteractor worldInteractor in _worldInteractors)
            {
                worldInteractor.AddDeactivationInput();
            }
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