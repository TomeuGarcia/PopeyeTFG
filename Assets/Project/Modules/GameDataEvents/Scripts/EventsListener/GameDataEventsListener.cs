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
            string position = "", string deathCause = "", string enemyType = "", 
            string actionType = "", string playerCurrentHealth = "")
        {
            string content = "";

            content += eventName + CONTENT_SEPARATOR;

            return content;
        }

        private void OnPlayerDeath(OnPlayerDeathEvent eventInfo)
        {
            PlayerDeathEventData eventData = 
                new PlayerDeathEventData(GetNewGenericEventData(), eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: PlayerDeathEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                position: eventData.Position.ToString(),
                deathCause: eventData.DamageHitName
                );

            _eventsConsumer.AddEventContent(eventContent);
        }

        private void OnPlayerTakeDamage(OnPlayerTakeDamageEvent eventInfo)
        {
            PlayerTakeDamageEventData eventData =
                new PlayerTakeDamageEventData(GetNewGenericEventData(), eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: PlayerTakeDamageEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                position: eventData.Position.ToString(),
                enemyType: eventData.DamageHitName
                );

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


        
        // ...
        
    }
    
    
    
}