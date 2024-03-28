using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public struct OnPlayerActionEvent
    {
        public Vector3 Position { get; private set; }
        public PlayerStates State { get; private set; }

        public OnPlayerActionEvent(Vector3 position, PlayerStates state)
        {
            Position = position;
            State = state;
        }
    }
    
    public class PlayerActionEventData
    {
        public const string NAME = "Player Action";
        public GenericEventData GenericEventData { get; private set; }
        public Vector3 Position { get; private set; }
        public string StateName { get; private set; }

        public PlayerActionEventData(GenericEventData genericEventData, OnPlayerActionEvent eventInfo)
        {
            GenericEventData = genericEventData;
            Position = eventInfo.Position;
            StateName = eventInfo.State.ToString();
        }
    }
}