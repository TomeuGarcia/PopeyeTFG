using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents.EventsUtilities
{
    
    [System.Serializable]
    public class EventTypeToEventDispatch
    {
        [SerializeField] private EventTypes _eventType = EventTypes.OnPuzzleEnter;

        private enum EventTypes
        {
            OnPuzzleEnter,
            OnPuzzleExit
        }
        
        
        public void Dispatch(GameObject sourceGameObject)
        {
            IEventSystemService eventSystemService = ServiceLocator.Instance.GetService<IEventSystemService>();

            if (_eventType == EventTypes.OnPuzzleEnter)
            {
                eventSystemService.Dispatch(new OnPuzzleEnterEvent(sourceGameObject.name));
            }
            else if (_eventType == EventTypes.OnPuzzleExit)
            {
                eventSystemService.Dispatch(new OnPuzzleExitEvent(sourceGameObject.name));
            }
        }


    }
}