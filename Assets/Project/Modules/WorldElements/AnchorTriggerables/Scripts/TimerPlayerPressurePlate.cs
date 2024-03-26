using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Popeye.Modules.WorldElements.WorldInteractors;
using Popeye.Scripts.ObjectTypes;
using UnityEngine;
using Timer = Popeye.Timers.Timer;

namespace Popeye.Modules.WorldElements.AnchorTriggerables
{


    public class TimerPlayerPressurePlate : MonoBehaviour
    {
        [Header("REFERENCES")] 
        [SerializeField] private MeshRenderer _buttonMesh;
        [SerializeField] private Collider _collider;

        private Material _timerMaterial;
        [SerializeField] private Transform _buttonTransform;

        [Header("TIMER")] 
        [SerializeField, Range(0.0f, 30.0f)] private float _pressedDuration = 3.0f;

        [SerializeField] private bool _triggerAllOnce = false;
        private bool _triggeredAllOnceAlready;


        [Header("WORLD INTERACTORS")] 
        [SerializeField] private AWorldInteractor[] _worldInteractors;

        private int _triggeredCount;

        private Coroutine _countdownCoroutine = null;
        private bool CountdownCoroutineIsActive => _countdownCoroutine != null;


        [Header("ACCEPT TYPES")] 
        [SerializeField] private ObjectTypeAsset[] _acceptTypes;
        private HashSet<GameObject> _acceptedGameObjects = new HashSet<GameObject>(2);


        private void OnEnable()
        {
            if (_triggerAllOnce)
            {
                foreach (AWorldInteractor worldInteractor in _worldInteractors)
                {
                    worldInteractor.OnEnterActivated += CancelTimerAndButton;
                }
            }
        }

        private void OnDisable()
        {
            if (_triggerAllOnce)
            {
                foreach (AWorldInteractor worldInteractor in _worldInteractors)
                {
                    worldInteractor.OnEnterActivated -= CancelTimerAndButton;
                }
            }
        }


        private void Awake()
        {
            _triggeredCount = 0;

            _timerMaterial = _buttonMesh.material;

            _triggeredAllOnceAlready = false;
            SetFillValue(0);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (_triggeredAllOnceAlready) return;
            
            if (AcceptsOtherCollider(other, true))
            {
                if (++_triggeredCount > 1) return;

                
                SetFillValue(1);
                if (CountdownCoroutineIsActive)
                {
                    StopCoroutine(_countdownCoroutine);
                }
                else
                {
                    PlayTriggerAnimation();
                    ActivateWorldInteractors();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_triggeredAllOnceAlready) return;

            if (AcceptsOtherCollider(other, false))
            {
                if (--_triggeredCount > 0) return;
                
                _countdownCoroutine = StartCoroutine(StartCountdownTimer());
            }
        }

        private bool AcceptsOtherCollider(Collider other, bool addReferenceIfAccepts)
        {
            if (!other.TryGetComponent(out IObjectType otherObjectType)) return false;
            
            if (_acceptedGameObjects.Contains(other.gameObject) && addReferenceIfAccepts) return false;
            if (addReferenceIfAccepts)
            {
                _acceptedGameObjects.Add(other.gameObject);
            }
            else
            {
                _acceptedGameObjects.Remove(other.gameObject);
            }
            
            return otherObjectType.IsOfAnyType(_acceptTypes);
        }


        


        protected void PlayTriggerAnimation()
        {
            _buttonTransform.DOLocalMove(Vector3.down * 0.05f, 0.2f);
        }

        protected void PlayUntriggerAnimation()
        {
            _buttonTransform.DOLocalMove(Vector3.up * 0.05f, 0.2f);
        }


        private IEnumerator StartCountdownTimer()
        {
            Timer pressedTimer = new Timer(_pressedDuration);
            while (!pressedTimer.HasFinished())
            {
                pressedTimer.Update(Time.deltaTime);
                SetFillValue(1-pressedTimer.GetCounterRatio01());
                
                yield return null;
            }
            
            
            if (!_triggeredAllOnceAlready)
            {
                DeactivateWorldInteractors();
                PlayUntriggerAnimation();
            }

            _countdownCoroutine = null;
            RefreshCollider().Forget();
        }

        private void SetFillValue(float fillValue)
        {
            _timerMaterial.SetFloat("_FillT", fillValue);
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


        private void CancelTimerAndButton()
        {
            _triggeredAllOnceAlready = true;
            
            if (CountdownCoroutineIsActive)
            {
                SetFillValue(1);
                StopCoroutine(_countdownCoroutine);
            }
        }

        private async UniTaskVoid RefreshCollider()
        {
            _collider.enabled = false;
            await UniTask.Yield();
            _triggeredCount = 0;
            _collider.enabled = true;
        }
        
    }
}