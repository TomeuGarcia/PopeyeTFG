using System.Collections.Generic;
using Popeye.Modules.Enemies.General;
using Popeye.Modules.PlayerAnchor.Player;

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

        
        private void OnEnemyWavesSpawnerStart(OnEnemyWavesSpawnerStartEvent eventInfo)
        {
            EnemyWavesSpawnerStartEventData eventData =
                new EnemyWavesSpawnerStartEventData(GetNewGenericEventData(), eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: EnemyWavesSpawnerStartEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                id: eventData.Id,
                wavesQuantity: eventData.NumberOfWaves.ToString());

            _eventsConsumer.AddEventContent(eventContent);
        }
        
        private void OnEnemyWaveStart(OnEnemyWaveStartEvent eventInfo)
        {
            EnemyWaveStartEventData eventData =
                new EnemyWaveStartEventData(GetNewGenericEventData(), eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: EnemyWaveStartEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                id: eventData.Id,
                enemiesQuantities: ContentFromEnemiesToQuantities(eventData.EnemyIdsToQuantities));

            _eventsConsumer.AddEventContent(eventContent);
        }

        private void OnAllEnemyWavesCompleted(OnAllEnemyWavesCompletedEvent eventInfo)
        {
            AllEnemyWavesCompletedEventData eventData =
                new AllEnemyWavesCompletedEventData(GetNewGenericEventData(),eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: AllEnemyWavesCompletedEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                id: eventData.Id);

            _eventsConsumer.AddEventContent(eventContent);
        }

       

        private void OnEnemyTakeDamage(OnEnemyTakeDamageEvent eventInfo)
        {
            EnemyTakeDamageEventData eventData =
                new EnemyTakeDamageEventData(GetNewGenericEventData(), eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: EnemyTakeDamageEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                enemyType: eventData.EnemyName,
                damageCause: eventData.DamageHitName,
                wasKilled: eventData.WasKilled.ToString());

            _eventsConsumer.AddEventContent(eventContent);
        }
        
        private void OnEnemyKilledByDamage(OnEnemyKilledByDamageEvent eventInfo)
        {
            EnemyKilledByDamageEventData eventData =
                new EnemyKilledByDamageEventData(GetNewGenericEventData(), eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: EnemyKilledByDamageEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                enemyType: eventData.EnemyName,
                damageCause: eventData.DamageHitName,
                playerActionsQuantities: ContentFromTrackedPlayerActions(eventData.TrackedPlayerActions));

            _eventsConsumer.AddEventContent(eventContent);
        }

        
        
        
        
        private string ContentFromEnemiesToQuantities(Dictionary<EnemyID, int> enemiesToQuantities)
        {
            string contentData = "";

            foreach (KeyValuePair<EnemyID, int> enemyToQuantity in enemiesToQuantities)
            {
                contentData += enemyToQuantity.Value + ";";
            }
            contentData.Remove(contentData.Length - 1);

            return contentData;
        }
        
        private string ContentFromTrackedPlayerActions(Dictionary<PlayerMovesetActions, int> trackedPlayerActions)
        {
            string contentData = "";

            foreach (KeyValuePair<PlayerMovesetActions, int> enemyToQuantity in trackedPlayerActions)
            {
                contentData += enemyToQuantity.Value + ";";
            }
            contentData.Remove(contentData.Length - 1);

            return contentData;
        }
        
    }
}