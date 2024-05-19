namespace Popeye.Modules.GameDataEvents
{
    public struct OnPuzzleExitEvent
    {
        public string Id { get; private set; }

        public OnPuzzleExitEvent(string id)
        {
            Id = id;
        }
    }
    
    public class PuzzleExitEventData
    {
        public const string NAME = "Exit Puzzle";
        public GenericEventData GenericEventData { get; private set; }
        public string Id { get; private set; }

        public PuzzleExitEventData(GenericEventData genericEventData, OnPuzzleExitEvent eventInfo)
        {
            GenericEventData = genericEventData;
            Id = eventInfo.Id;
        }
    }
}