using System;
using NaughtyAttributes;
using Popeye.Scripts.ObjectTypes;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents.EventsUtilities
{
    [RequireComponent(typeof(ObjectTypeTriggerEnter))]
    public class EventDispatchTrigger : MonoBehaviour
    {
        [Header("TRIGGER")]
        [Required()] [SerializeField] private ObjectTypeTriggerEnter _triggerEnter;

        [Header("EVENT")] 
        [SerializeField] private EventTypeToEventDispatch _eventDispatch;


        private void OnEnable()
        {
            _triggerEnter.OnGameObjectEnters += OnGameObjectEntersEvent;
        }
        private void OnDisable()
        {
            _triggerEnter.OnGameObjectEnters -= OnGameObjectEntersEvent;
        }


        private void OnGameObjectEntersEvent(GameObject acceptedGameObject)
        {
            _eventDispatch.Dispatch(gameObject);
        }
        
    }
}