using Popeye.Modules.Enemies.General;


namespace Popeye.Modules.GameDataEvents
{
    public struct OnEnemySeesPlayerEvent
    {
        public EnemyID Id { get; private set; }

        public OnEnemySeesPlayerEvent(EnemyID id)
        {
            Id = id;
        }
    }
    
    public class EnemySeesPlayerEventData
    {
        public const string NAME = "Enemy Sees Player";
        public GenericEventData GenericEventData { get; private set; }
        public string EnemyName { get; private set; }


        public EnemySeesPlayerEventData(GenericEventData genericEventData, OnEnemySeesPlayerEvent eventInfo)
        {
            GenericEventData = genericEventData;
            EnemyName = eventInfo.Id.GetEnemyName();
        }
    }
    
    
}