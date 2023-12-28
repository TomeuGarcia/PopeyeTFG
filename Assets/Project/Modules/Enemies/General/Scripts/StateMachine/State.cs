using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Popeye.Modules.Enemies.StateMachine
{
    public class State : MonoBehaviour, IState
    {
        [SerializeField] private List<StateTransition> _transitions = new List<StateTransition>();
        [SerializeField] private UnityEvent _onEnterState = new UnityEvent();
        [SerializeField] private UnityEvent _onExitState = new UnityEvent();

        public IState ProcessTransitions()
        {
            // Loop over all of the possible transitions from this state
            foreach (var transition in _transitions)
            {
                // Check to see if the particular transition conditions are met
                if (transition.ShouldTransition())
                {
                    // Let the caller know which state we should transition to
                    return transition.NextState;
                }
            }

            // No transitions have all of their conditions met
            return null;
        }

        private void OnEnable()
        {
            _onEnterState.Invoke();
        }

        private void OnDisable()
        {
            _onExitState.Invoke();
        }

        public void Enter() => gameObject.SetActive(true);

        public void Exit() => gameObject.SetActive(false);
    }
}

