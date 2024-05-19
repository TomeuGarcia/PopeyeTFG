using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Enemies.General;
using Popeye.Modules.PlayerAnchor.Player;

namespace Popeye.Modules.GameDataEvents
{
    public partial class GameDataEventsListener
    {
        private readonly IEventSystemService _eventSystemService;
        private readonly IGameDataEventsConsumer _eventsConsumer;
        private readonly IActiveSceneDataEventsProvider _activeSceneDataEventsProvider;
        
        private string _emptyEnemiesQuantities;
        private string _emptyPlayerActionsQuantities;
        
        public GameDataEventsListener(IEventSystemService eventSystemService, IGameDataEventsConsumer eventsConsumer,
            IActiveSceneDataEventsProvider activeSceneDataEventsProvider)
        {
            _eventSystemService = eventSystemService;
            _eventsConsumer = eventsConsumer;
            _activeSceneDataEventsProvider = activeSceneDataEventsProvider;
            
            MakeContentHeaders();
            SetupEmptyEnemiesQuantities();
            SetupEmptyPlayerActionsQuantities();
        }


        public void StartListening()
        {
            _eventSystemService.Subscribe<OnPlayerRestEvent>(OnPlayerRest);
            _eventSystemService.Subscribe<OnPlayerTakeDamageEvent>(OnPlayerTakeDamage);
            _eventSystemService.Subscribe<OnPlayerKilledByDamageEvent>(OnPlayerKilledByDamage);
            _eventSystemService.Subscribe<OnPlayerHealEvent>(OnPlayerHeal);
            _eventSystemService.Subscribe<OnPlayerUpdateEvent>(OnPlayerUpdate);
            _eventSystemService.Subscribe<OnPlayerActionEvent>(OnPlayerAction);
            
            _eventSystemService.Subscribe<OnPuzzleEnterEvent>(OnEnterPuzzle);
            _eventSystemService.Subscribe<OnPuzzleExitEvent>(OnExitPuzzle);
            
            _eventSystemService.Subscribe<OnEnemySeesPlayerEvent>(OnEnemySeesPlayer);
            _eventSystemService.Subscribe<OnEnemyWavesSpawnerStartEvent>(OnEnemyWavesSpawnerStart);
            _eventSystemService.Subscribe<OnEnemyWaveStartEvent>(OnEnemyWaveStart);
            _eventSystemService.Subscribe<OnAllEnemyWavesCompletedEvent>(OnAllEnemyWavesCompleted);
            
            _eventSystemService.Subscribe<OnEnemyTakeDamageEvent>(OnEnemyTakeDamage);
            _eventSystemService.Subscribe<OnEnemyKilledByDamageEvent>(OnEnemyKilledByDamage);
        }
        
        public void StopListening()
        {
            _eventSystemService.Unsubscribe<OnPlayerRestEvent>(OnPlayerRest);
            _eventSystemService.Unsubscribe<OnPlayerTakeDamageEvent>(OnPlayerTakeDamage);
            _eventSystemService.Unsubscribe<OnPlayerKilledByDamageEvent>(OnPlayerKilledByDamage);
            _eventSystemService.Unsubscribe<OnPlayerHealEvent>(OnPlayerHeal);
            _eventSystemService.Unsubscribe<OnPlayerUpdateEvent>(OnPlayerUpdate);
            _eventSystemService.Unsubscribe<OnPlayerActionEvent>(OnPlayerAction);
            
            _eventSystemService.Unsubscribe<OnPuzzleEnterEvent>(OnEnterPuzzle);
            _eventSystemService.Unsubscribe<OnPuzzleExitEvent>(OnExitPuzzle);
            
            _eventSystemService.Unsubscribe<OnEnemySeesPlayerEvent>(OnEnemySeesPlayer);
            _eventSystemService.Unsubscribe<OnEnemyWavesSpawnerStartEvent>(OnEnemyWavesSpawnerStart);
            _eventSystemService.Unsubscribe<OnEnemyWaveStartEvent>(OnEnemyWaveStart);
            _eventSystemService.Unsubscribe<OnAllEnemyWavesCompletedEvent>(OnAllEnemyWavesCompleted);
            
            _eventSystemService.Unsubscribe<OnEnemyTakeDamageEvent>(OnEnemyTakeDamage);
            _eventSystemService.Unsubscribe<OnEnemyKilledByDamageEvent>(OnEnemyKilledByDamage);
        }



        private GenericEventData GetNewGenericEventData()
        {
            return new GenericEventData(_activeSceneDataEventsProvider.GetActiveSceneName());
        }


        private string MakeContentFromEventData(string eventName, string timeStamp, string sceneName, 
            string position = " " + EventsParseHelper.CONTENT_SEPARATOR + " " + EventsParseHelper.CONTENT_SEPARATOR + " ", 
            string damageCause = " ", string enemyType = " ", 
            string playerActionType = " ", string playerHealthCurrent = " ", string playerHealthBeforeEvent = " ", 
            string wasKilled = " ",
            string id = " ", string wavesQuantity = " ", 
            string enemiesQuantities = null, string playerActionsQuantities = null)
        {
            string content = "";

            ConcatenateDataToContent(ref content, eventName);
            ConcatenateDataToContent(ref content, timeStamp);
            ConcatenateDataToContent(ref content, sceneName);
            ConcatenateDataToContent(ref content, position);
            ConcatenateDataToContent(ref content, damageCause);
            ConcatenateDataToContent(ref content, enemyType);
            ConcatenateDataToContent(ref content, playerActionType);
            ConcatenateDataToContent(ref content, playerHealthCurrent);
            ConcatenateDataToContent(ref content, playerHealthBeforeEvent);
            ConcatenateDataToContent(ref content, wasKilled);
            
            ConcatenateDataToContent(ref content, id);
            ConcatenateDataToContent(ref content, wavesQuantity);
            
            if (enemiesQuantities == null)
            {
                enemiesQuantities = _emptyEnemiesQuantities;
            }
            ConcatenateDataToContent(ref content, enemiesQuantities);

            if (playerActionsQuantities == null)
            {
                playerActionsQuantities = _emptyPlayerActionsQuantities;
            }
            ConcatenateLastDataToContent(ref content, playerActionsQuantities);

            return content;
        }

        private void ConcatenateDataToContent(ref string content, string data)
        {
            content += data + EventsParseHelper.CONTENT_SEPARATOR;
        }
        private void ConcatenateLastDataToContent(ref string content, string data)
        {
            content += data;
        }
        
        private void MakeContentHeaders()
        {
            string content = MakeContentFromEventData(
                eventName: "Event Name",
                timeStamp: "Time Stamp",
                sceneName: "Scene Name",
                position: "PosX" + EventsParseHelper.CONTENT_SEPARATOR + "PosY" + EventsParseHelper.CONTENT_SEPARATOR + "PosZ",
                damageCause: "Damage Cause",
                enemyType: "Enemy Type",
                playerActionType: "Player Action Type",
                playerHealthCurrent: "Current Player Health",
                playerHealthBeforeEvent: "Player Health Before Event",
                wasKilled: "Was Killed",
                id: "Id",
                wavesQuantity: "Waves Quantity",
                enemiesQuantities: GetEnemiesQuantitiesHeader(),
                playerActionsQuantities: GetPlayerActionsQuantitiesHeader()
            );
            
            _eventsConsumer.AddEventContent(content);
        }

        private string GetEnemiesQuantitiesHeader()
        {
            EnemyID[] enemyIds = ServiceLocator.Instance.GetService<IEnemyIDsCollectionService>().EnemyIDs;

            string headerContent = "";
            
            int i = 0;
            for (; i < enemyIds.Length-1; ++i)
            {
                headerContent += enemyIds[i].GetEnemyName() + " Quantity" + EventsParseHelper.CONTENT_SEPARATOR;
            }
            if (i < enemyIds.Length)
            {
                headerContent += enemyIds[i].GetEnemyName() + " Quantity";
            }
            
            return headerContent;
        }

        
        private void SetupEmptyEnemiesQuantities()
        {
            EnemyID[] enemyIds = ServiceLocator.Instance.GetService<IEnemyIDsCollectionService>().EnemyIDs;

            for (int i = 1; i < enemyIds.Length; ++i)
            {
                _emptyEnemiesQuantities += " " + EventsParseHelper.CONTENT_SEPARATOR;
            }
        }
        
        
        private string GetPlayerActionsQuantitiesHeader()
        {
            PlayerMovesetActions[] playerMovesetActions = PlayerMovesetActionsHelper.GetAllValuesArray();

            string headerContent = "";
            
            int i = 0;
            for (; i < playerMovesetActions.Length - 1; ++i)
            {
                headerContent += playerMovesetActions[i].ToString() + " Quantity" + EventsParseHelper.CONTENT_SEPARATOR;
            }

            if (i < playerMovesetActions.Length)
            {
                headerContent += playerMovesetActions[i].ToString() + " Quantity";
            }
            
            return headerContent;
        }
        
        private void SetupEmptyPlayerActionsQuantities()
        {
            PlayerMovesetActions[] playerMovesetActions = PlayerMovesetActionsHelper.GetAllValuesArray();

            for (int i = 1; i < playerMovesetActions.Length; ++i)
            {
                _emptyPlayerActionsQuantities += " " + EventsParseHelper.CONTENT_SEPARATOR;
            }
        }
        
    }
    
    
    
}