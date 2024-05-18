namespace Popeye.Modules.GameDataEvents
{
    public partial class GameDataEventsListener
    {
        private void OnEnemySeesPlayer(OnEnemySeesPlayerEvent eventInfo)
        {
            EnemySeesPlayerEventData eventData = 
                new EnemySeesPlayerEventData(GetNewGenericEventData(), eventInfo);


            string eventContent = MakeContentFromEventData(
                eventName:EnemySeesPlayerEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                enemyType: eventData.EnemyName);
            
            _eventsConsumer.AddEventContent(eventContent);
        }

        

        private void OnEnemyWaveStart(OnEnemyWaveStartEvent eventInfo)
        {
            EnemyWaveStartEventData eventData =
                new EnemyWaveStartEventData(GetNewGenericEventData(),eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: EnemyWaveStartEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName);

            _eventsConsumer.AddEventContent(eventContent);
        }

        private void OnAllEnemyWavesCompleted(OnAllEnemyWavesCompletedEvent eventInfo)
        {
            AllEnemyWavesCompletedEventData eventData =
                new AllEnemyWavesCompletedEventData(GetNewGenericEventData(),eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: AllEnemyWavesCompletedEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName);

            _eventsConsumer.AddEventContent(eventContent);
        }

       

        private void OnEnemyTakeDamage(OnEnemyTakeDamageEvent eventInfo)
        {
            EnemyTakeDamageEventData eventData =
                new EnemyTakeDamageEventData(GetNewGenericEventData(),eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: EnemyTakeDamageEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                enemyType: eventData.EnemyName,
                damageCause: eventData.DamageHitName,
                wasKilled: eventData.WasKilled.ToString());

            _eventsConsumer.AddEventContent(eventContent);
        }
    }
}