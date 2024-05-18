using Popeye.Modules.PlayerAnchor.Player;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public struct OnPlayerActionEvent
    {
        public Vector3 Position { get; private set; }
        public PlayerMovesetActions ActionName { get; private set; }
        

        public OnPlayerActionEvent(Vector3 position, PlayerMovesetActions actionName)
        {
            Position = position;
            ActionName = actionName;
        }
    }
    
    public class PlayerActionEventData
    {
        public const string NAME = "Player Action";
        public GenericEventData GenericEventData { get; private set; }
        public Vector3 Position { get; private set; }
        public string ActionName { get; private set; }

        public PlayerActionEventData(GenericEventData genericEventData, OnPlayerActionEvent eventInfo)
        {
            GenericEventData = genericEventData;
            Position = eventInfo.Position;
            ActionName = eventInfo.ActionName.ToString();
        }
    }
}