using System.Collections.Generic;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.PlayerAnchor.Player;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents.EventsUtilities
{
    public class PlayerActionEventsListener
    {
        private static PlayerMovesetActions[] _allPlayerMovesetActions = null;
        
        private readonly IEventSystemService _eventSystemService;
        private readonly Dictionary<PlayerMovesetActions, int> _trackedPlayerActions;
        public Dictionary<PlayerMovesetActions, int> TrackedPlayerActions => _trackedPlayerActions;
        

        public PlayerActionEventsListener()
        {
            _eventSystemService = ServiceLocator.Instance.GetService<IEventSystemService>();

            if (_allPlayerMovesetActions == null)
            {
                _allPlayerMovesetActions = PlayerMovesetActionsHelper.GetAllValuesArray();
            }

            _trackedPlayerActions = new Dictionary<PlayerMovesetActions, int>(_allPlayerMovesetActions.Length);
            foreach (PlayerMovesetActions playerMovesetAction in _allPlayerMovesetActions)
            {
                _trackedPlayerActions.Add(playerMovesetAction, 0);
            }
        }
        
        
        public void StartListeningAndClearState()
        {
            Debug.Log("START");
            _eventSystemService.Subscribe<OnPlayerActionEvent>(OnPlayerAction);
            ClearTrackedActions();
        }
        
        public void StopListening()
        {
            Debug.Log("STOP");
            _eventSystemService.Unsubscribe<OnPlayerActionEvent>(OnPlayerAction);
        }
        
        private void ClearTrackedActions()
        {
            foreach (PlayerMovesetActions playerMovesetAction in _allPlayerMovesetActions)
            {
                _trackedPlayerActions[playerMovesetAction] = 0;
            }
        }
        
        private void OnPlayerAction(OnPlayerActionEvent eventInfo)
        {
            Debug.Log(eventInfo.ActionName);
            _trackedPlayerActions[eventInfo.ActionName] += 1;
        }
        
    }
}