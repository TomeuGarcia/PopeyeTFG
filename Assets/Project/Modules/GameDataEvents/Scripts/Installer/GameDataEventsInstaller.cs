using Popeye.Core.Services.EventSystem;
using Popeye.Scripts.Core.Scenes;
using Popeye.Scripts.Core.Scenes.PlayedScene;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public class GameDataEventsInstaller : MonoBehaviour
    {
        [SerializeField] private GameDataEventsDispatchTester _eventsDispatchTester;
        [SerializeField] private GameDataEventsCSVSaverConfig _csvSaverConfig;

        [SerializeField] private SceneReferenceAsset _ignoreScene;
        
        private IEventSystemService _eventSystemService;
        
        private LastLoadedSceneDataEventsProvider _activeSceneDataEventsProvider;
        private GameDataEventsListener _eventsListener;
        private GameDataEventsCSVSaver _gameDataEventsCSVSaver;

        public void Install(IEventSystemService eventSystemService, 
            ICurrentlyPlayedSceneProvider currentlyPlayedSceneProvider)
        {
            _eventSystemService = eventSystemService;
        
            _activeSceneDataEventsProvider = 
                new LastLoadedSceneDataEventsProvider(currentlyPlayedSceneProvider);
            
            _gameDataEventsCSVSaver = 
                new GameDataEventsCSVSaver(_csvSaverConfig);
                
            _eventsListener = 
                new GameDataEventsListener(eventSystemService, _gameDataEventsCSVSaver, _activeSceneDataEventsProvider);
            
            _eventsDispatchTester.Init(eventSystemService);
            
            
            _eventSystemService.Subscribe<ISceneLoadManager.OnStartLoadingAdditiveSceneEvent>(OnFirstSceneLoaded);
        }



        private void OnFirstSceneLoaded(ISceneLoadManager.OnStartLoadingAdditiveSceneEvent eventData)
        {
            if (ReferenceEquals(eventData.SceneReference, _ignoreScene))
            {
                return;
            }
            
            _eventSystemService.Unsubscribe<ISceneLoadManager.OnStartLoadingAdditiveSceneEvent>(OnFirstSceneLoaded);
            StartListeningToEvents(eventData.SceneReference);
        }
        
        private void StartListeningToEvents(ISceneReference startScene)
        {
            _eventsListener.StartListening();
            
            _gameDataEventsCSVSaver.Start();
        }
        
        public void Uninstall()
        {
            _eventsListener.StopListening();
            _gameDataEventsCSVSaver.Finish();
        }
    }
}