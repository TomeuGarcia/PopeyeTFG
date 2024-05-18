using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public struct OnPuzzleEnterEvent
    {
        public string Id { get; private set; }

        public OnPuzzleEnterEvent(string id)
        {
            Id = id;
        }
    }
    
    public class PuzzleEnterEventData
    {
        public const string NAME = "Enter Puzzle";
        public GenericEventData GenericEventData { get; private set; }
        public string Id { get; private set; }

        public PuzzleEnterEventData(GenericEventData genericEventData, OnPuzzleEnterEvent eventInfo)
        {
            GenericEventData = genericEventData;
            Id = eventInfo.Id;
        }
    }
}