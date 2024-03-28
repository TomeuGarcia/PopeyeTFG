using Popeye.Core.Services.EventSystem;

namespace Popeye.Modules.GameDataEvents
{
    public class GameDataEventsListener
    {
        private readonly IEventSystemService _eventSystemService;
        private readonly IGameDataEventsConsumer _eventsConsumer;
        private readonly IActiveSceneDataEventsProvider _activeSceneDataEventsProvider;

        private const string CONTENT_SEPARATOR = ",";
        

        public GameDataEventsListener(IEventSystemService eventSystemService, IGameDataEventsConsumer eventsConsumer,
            IActiveSceneDataEventsProvider activeSceneDataEventsProvider)
        {
            _eventSystemService = eventSystemService;
            _eventsConsumer = eventsConsumer;
            _activeSceneDataEventsProvider = activeSceneDataEventsProvider;
        }


        public void StartListening()
        {
            _eventSystemService.Subscribe<OnEnemySeesPlayerEvent>(OnEnemySeesPlayer);
            // ...
        }
        
        public void StopListening()
        {
            _eventSystemService.Unsubscribe<OnEnemySeesPlayerEvent>(OnEnemySeesPlayer);
            // ...
        }



        private GenericEventData GetNewGenericEventData()
        {
            return new GenericEventData(_activeSceneDataEventsProvider.GetActiveSceneName());
        }


        private string MakeContentFromEventData(string eventName, string timeStamp, string sceneName, 
            string position = "", string damageCause = "", string enemyType = "", 
            string actionType = "", string playerHealthCurrent = "", string playerHealthBeforeEvent = "", string wasKilled = "")
        {
            string content = "";

            content += eventName + CONTENT_SEPARATOR;

            return content;
        }


        //OnPlayerRestEvent TODO when implemented rest mechanic

        private void OnPlayerTakeDamage(OnPlayerTakeDamageEvent eventInfo)
        {
            PlayerTakeDamageEventData eventData =
                new PlayerTakeDamageEventData(GetNewGenericEventData(), eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: PlayerTakeDamageEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                position: eventData.Position.ToString(),
                damageCause: eventData.DamageHitName,
                playerHealthCurrent: eventData.CurrentHealth.ToString(),
                wasKilled: eventData.WasKilled.ToString()
                );

            _eventsConsumer.AddEventContent(eventContent);
        }

        private void OnPlayerHeal(OnPlayerHealEvent eventInfo)
        {
            PlayerHealEventData eventData =
                new PlayerHealEventData(GetNewGenericEventData(), eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: PlayerHealEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                position: eventData.Position.ToString(),
                playerHealthCurrent: eventData.CurrentHealth.ToString(),
                playerHealthBeforeEvent: "" /*TODO*/);

            _eventsConsumer.AddEventContent(eventContent);
        }

        private void OnPlayerUpdate(OnPlayerUpdateEvent eventInfo)
        {
            PlayerUpdateEventData eventData =
                new PlayerUpdateEventData(GetNewGenericEventData(), eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: PlayerUpdateEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                position: eventData.Position.ToString());

            _eventsConsumer.AddEventContent(eventContent);
        }

        private void OnPlayerAction(OnPlayerActionEvent eventInfo)
        {
            PlayerActionEventData eventData =
                new PlayerActionEventData(GetNewGenericEventData(), eventInfo);


            string eventContent = MakeContentFromEventData(
                eventName: PlayerActionEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                actionType: eventData.ActionName);

            _eventsConsumer.AddEventContent(eventContent);
        }

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

        

        private void OnWaveStart(OnEnemyWaveStartEvent eventInfo)
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
                actionType: eventData.DamageHitName,
                wasKilled: eventData.WasKilled.ToString());

            _eventsConsumer.AddEventContent(eventContent);
        }

        
        
        // ...
        
    }
    
    
    
}