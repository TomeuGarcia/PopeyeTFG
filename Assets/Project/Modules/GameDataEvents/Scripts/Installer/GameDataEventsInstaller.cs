using Popeye.Core.Services.EventSystem;
using Popeye.Scripts.Core.Scenes;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public class GameDataEventsInstaller : MonoBehaviour
    {
        [SerializeField] private GameDataEventsDispatchTester _eventsDispatchTester;
        [SerializeField] private GameDataEventsCSVSaverConfig _csvSaverConfig;

        private IEventSystemService _eventSystemService;
        
        private LastLoadedSceneDataEventsProvider _activeSceneDataEventsProvider;
        private GameDataEventsListener _eventsListener;
        private GameDataEventsCSVSaver _gameDataEventsCSVSaver;

        public void Install(IEventSystemService eventSystemService)
        {
            _eventSystemService = eventSystemService;
        
            _activeSceneDataEventsProvider = 
                new LastLoadedSceneDataEventsProvider(eventSystemService);
            
            _gameDataEventsCSVSaver = 
                new GameDataEventsCSVSaver(_csvSaverConfig);
                
            _eventsListener = 
                new GameDataEventsListener(eventSystemService, _gameDataEventsCSVSaver, _activeSceneDataEventsProvider);
            
            _eventsDispatchTester.Init(eventSystemService);
            
            
            _eventSystemService.Subscribe<ISceneLoadManager.OnStartLoadingAdditiveSceneEvent>(OnFirstSceneLoaded);
        }



        private void OnFirstSceneLoaded(ISceneLoadManager.OnStartLoadingAdditiveSceneEvent eventData)
        {
            _eventSystemService.Unsubscribe<ISceneLoadManager.OnStartLoadingAdditiveSceneEvent>(OnFirstSceneLoaded);
            StartListeningToEvents();
        }
        
        private void StartListeningToEvents()
        {
            _activeSceneDataEventsProvider.StartListening();
            _eventsListener.StartListening();
            
            _gameDataEventsCSVSaver.Start();
        }
        
        public void Uninstall()
        {
            _activeSceneDataEventsProvider.StopListening();
            _eventsListener.StopListening();
            _gameDataEventsCSVSaver.Finish();
        }
    }
}