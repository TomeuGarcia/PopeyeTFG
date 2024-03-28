
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public struct OnPlayerUpdateEvent
    {
        public Vector3 Position { get; private set; }

        public OnPlayerUpdateEvent(Vector3 position)
        {
            Position = position;
        }
    }
    
    public class PlayerUpdateEventData
    {
        public const string NAME = "Player Update";
        public GenericEventData GenericEventData { get; private set; }
        public Vector3 Position { get; private set; }

        public PlayerUpdateEventData(GenericEventData genericEventData, OnPlayerUpdateEvent eventInfo)
        {
            GenericEventData = genericEventData;
            Position = eventInfo.Position;
        }
    }
}