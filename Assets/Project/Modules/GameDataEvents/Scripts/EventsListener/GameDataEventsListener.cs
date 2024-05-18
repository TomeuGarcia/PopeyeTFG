using Popeye.Core.Services.EventSystem;

namespace Popeye.Modules.GameDataEvents
{
    public partial class GameDataEventsListener
    {
        private readonly IEventSystemService _eventSystemService;
        private readonly IGameDataEventsConsumer _eventsConsumer;
        private readonly IActiveSceneDataEventsProvider _activeSceneDataEventsProvider;

        private const string CONTENT_SEPARATOR = ";";
        

        public GameDataEventsListener(IEventSystemService eventSystemService, IGameDataEventsConsumer eventsConsumer,
            IActiveSceneDataEventsProvider activeSceneDataEventsProvider)
        {
            _eventSystemService = eventSystemService;
            _eventsConsumer = eventsConsumer;
            _activeSceneDataEventsProvider = activeSceneDataEventsProvider;
            
            MakeContentHeaders();
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
            _eventSystemService.Subscribe<OnEnemyWaveStartEvent>(OnEnemyWaveStart);
            _eventSystemService.Subscribe<OnAllEnemyWavesCompletedEvent>(OnAllEnemyWavesCompleted);
            _eventSystemService.Subscribe<OnEnemyTakeDamageEvent>(OnEnemyTakeDamage);
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
            _eventSystemService.Unsubscribe<OnEnemyWaveStartEvent>(OnEnemyWaveStart);
            _eventSystemService.Unsubscribe<OnAllEnemyWavesCompletedEvent>(OnAllEnemyWavesCompleted);
            _eventSystemService.Unsubscribe<OnEnemyTakeDamageEvent>(OnEnemyTakeDamage);
        }



        private GenericEventData GetNewGenericEventData()
        {
            return new GenericEventData(_activeSceneDataEventsProvider.GetActiveSceneName());
        }


        private string MakeContentFromEventData(string eventName, string timeStamp, string sceneName, 
            string position = " ; ; ", string damageCause = " ", string enemyType = " ", 
            string playerActionType = " ", string playerHealthCurrent = " ", string playerHealthBeforeEvent = " ", 
            string wasKilled = " ",
            string id = " ", string wavesQuantity = " ", string slimesQuantity = " ", string slimeExplosiveQuantity = " ",
            string turretQuantity = " ", string turretVariationQuantity = " ", string shieldedQuantity = " ",
            string dashToQuantity = " ", string dashDropQuantity = " ", string slamQuantity = " ",
            string throwQuantity = " ", string pullQuantity = " ", string spinQuantity = " ", string spikesQuantity = " ")
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
            ConcatenateDataToContent(ref content, slimesQuantity);
            ConcatenateDataToContent(ref content, slimeExplosiveQuantity);
            ConcatenateDataToContent(ref content, turretQuantity);
            ConcatenateDataToContent(ref content, turretVariationQuantity);
            ConcatenateDataToContent(ref content, shieldedQuantity);
            ConcatenateDataToContent(ref content, dashToQuantity);
            ConcatenateDataToContent(ref content, dashDropQuantity);
            ConcatenateDataToContent(ref content, slamQuantity);
            ConcatenateDataToContent(ref content, throwQuantity);
            ConcatenateDataToContent(ref content, pullQuantity);
            ConcatenateDataToContent(ref content, spinQuantity);
            ConcatenateLastDataToContent(ref content, spikesQuantity);

            return content;
        }

        private void ConcatenateDataToContent(ref string content, string data)
        {
            content += data + CONTENT_SEPARATOR;
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
                position: "PosX" + CONTENT_SEPARATOR +"PosY" + CONTENT_SEPARATOR + "PosZ",
                damageCause: "Damage Cause",
                enemyType: "Enemy Type",
                playerActionType: "Player Action Type",
                playerHealthCurrent: "Current Player Health",
                playerHealthBeforeEvent: "Player Health Before Event",
                wasKilled: "Was Killed",
                id: "Id",
                wavesQuantity: "Waves Quantity",
                slimesQuantity: "Slime Quantity",
                slimeExplosiveQuantity: "Slime Explosive Quantity",
                turretQuantity: "Turret Quantity",
                turretVariationQuantity: "Turret Variation Quantity",
                shieldedQuantity: "Shielded Quantity",
                dashToQuantity: "Dash To Quantity",
                dashDropQuantity: "Dash Drop Quantity",
                slamQuantity: "Slam Quantity",
                throwQuantity: "Throw Quantity",
                pullQuantity: "Pull Quantity",
                spinQuantity: "Spin Quantity",
                spikesQuantity: "Spikes Quantity"
            );
            
            _eventsConsumer.AddEventContent(content);
        }
        
        
    }
    
    
    
}