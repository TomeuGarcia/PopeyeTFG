using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace Popeye.Modules.WorldElements.WorldInteractors
{
    public abstract class AWorldInteractor : MonoBehaviour
    {
        [Header("ACTIVATION INPUT COUNT")]
        [SerializeField, Range(1, 10)] private int _activationInputsCount = 1;
        private int _currentActivationInputsCount = 0;
        public int ActivationInputsCount => _activationInputsCount;

        public delegate void AWorldInteractorEvent();
        public AWorldInteractorEvent OnEnterActivated;


        private void Awake()
        {
            DoAwake();
        }


        public void AddActivationInput()
        {
            if (++_currentActivationInputsCount == _activationInputsCount)
            {
                EnterActivatedState();
            }        
        }
    
        public void AddDeactivationInput()
        {
            if (--_currentActivationInputsCount == _activationInputsCount - 1)
            {
                EnterDeactivatedState();
            }
        }

        public bool IsActivated()
        {
            return _currentActivationInputsCount == _activationInputsCount;
        }


        public void EnterActivatedState()
        {
            DoEnterActivatedState();
            OnEnterActivated?.Invoke();
        }

        public void EnterDeactivatedState()
        {
            DoEnterDeactivatedState();
        }

        protected abstract void DoAwake();
        protected abstract void DoEnterActivatedState();
        protected abstract void DoEnterDeactivatedState();

        public virtual async UniTask EnterActivatedStateAwait()
        {
            await UniTask.Yield();
        }
    }
    
}


