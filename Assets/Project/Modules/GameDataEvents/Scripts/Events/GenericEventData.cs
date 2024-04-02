namespace Popeye.Modules.GameDataEvents
{
    public class GenericEventData
    {
        private const string TIME_STAMP_FORMAT = "HH:mm:ss";
        
        public string TimeStamp { get; private set; }
        public string SceneName { get; private set; }
        

        public GenericEventData(string sceneName)
        {
            TimeStamp = System.DateTime.UtcNow.ToString(TIME_STAMP_FORMAT);
            SceneName = sceneName;
        }
    }
}