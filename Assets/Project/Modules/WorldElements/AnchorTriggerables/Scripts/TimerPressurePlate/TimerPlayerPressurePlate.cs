using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using AYellowpaper;
using Popeye.Modules.WorldElements.WorldInteractors;
using Popeye.Scripts.ObjectTypes;
using Project.Scripts.TweenExtensions;
using UnityEngine;
using Timer = Popeye.Timers.Timer;

namespace Popeye.Modules.WorldElements.AnchorTriggerables
{


    public class TimerPlayerPressurePlate : MonoBehaviour
    {
        [Header("AUDIO")]
        [SerializeField] private TimerButtonAudioPlayer _audio;
        
        [Header("VIEW")]
        [SerializeField] private InterfaceReference<ITimerPressurePlateView, MonoBehaviour> _view;
        public ITimerPressurePlateView View => _view.Value;
        
        [Header("REFERENCES")] 
        [SerializeField] private Transform _buttonTransform;
        [SerializeField] private Collider _collider;

        [SerializeField] private TweenConfigAsset _triggeredMoveBy;
        
        [Header("TIMER")] 
        [SerializeField, Range(0.0f, 30.0f)] private float _pressedDuration = 3.0f;
        [SerializeField, Range(0.0f, 5.0f)] private float _finalPressedDuration = 0.5f;

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
                    worldInteractor.OnEnterActivated += CancelAndLockTimerAndButton;
                }
            }
        }

        private void OnDisable()
        {
            if (_triggerAllOnce)
            {
                foreach (AWorldInteractor worldInteractor in _worldInteractors)
                {
                    worldInteractor.OnEnterActivated -= CancelAndLockTimerAndButton;
                }
            }
        }

        private void OnValidate()
        {
            _finalPressedDuration = Mathf.Min(_finalPressedDuration, _pressedDuration);
        }

        private void Awake()
        {
            _triggeredCount = 0;
            _triggeredAllOnceAlready = false;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (_triggeredAllOnceAlready) return;
            
            if (AcceptsOtherCollider(other, true))
            {
                if (++_triggeredCount > 1) return;

                
                _audio.PlayActivatedSound();
                
                if (CountdownCoroutineIsActive)
                {
                    StopCoroutine(_countdownCoroutine);
                    View.CancelTimerCountdown();
                    _audio.StopPlayingTickDown();
                }
                else
                {
                    PlayTriggerAnimation();
                    ActivateWorldInteractors();
                }
                
                View.SetTimerStart();
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
            _buttonTransform.BlendableLocalMoveBy(_triggeredMoveBy.Config);
        }

        protected void PlayUntriggerAnimation()
        {
            _buttonTransform.BlendableLocalMoveBy(_triggeredMoveBy.Config.Undo());
        }


        private IEnumerator StartCountdownTimer()
        {
            View.StartTimerCountdown(_pressedDuration, _finalPressedDuration);
            _audio.StartPlayingTickDown(_pressedDuration, _finalPressedDuration);
            
            yield return new WaitForSeconds(_pressedDuration);
            
            
            if (!_triggeredAllOnceAlready)
            {
                DeactivateWorldInteractors();
                PlayUntriggerAnimation();
            }

            _countdownCoroutine = null;
            RefreshCollider().Forget();
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


        private void CancelAndLockTimerAndButton()
        {
            _triggeredAllOnceAlready = true;
            
            if (CountdownCoroutineIsActive)
            {
                StopCoroutine(_countdownCoroutine);
                _countdownCoroutine = null;
            }
            
            View.CancelAndLockTimerCountdown();
            _audio.StopPlayingTickDown();
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