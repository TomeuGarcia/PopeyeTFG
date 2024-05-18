using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public struct OnPlayerRestEvent
    {
        public Vector3 Position { get; private set; }

        public OnPlayerRestEvent(Vector3 position)
        {
            Position = position;
        }
    }
    
    public class PlayerRestEventData
    {
        public const string NAME = "Player Rest";
        public GenericEventData GenericEventData { get; private set; }
        public Vector3 Position { get; private set; }

        public PlayerRestEventData(GenericEventData genericEventData, OnPlayerRestEvent eventInfo)
        {
            GenericEventData = genericEventData;
            Position = eventInfo.Position;
        }
    }
}