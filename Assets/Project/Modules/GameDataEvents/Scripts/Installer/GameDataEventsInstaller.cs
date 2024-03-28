using Popeye.Core.Services.EventSystem;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public class GameDataEventsInstaller : MonoBehaviour
    {
        [SerializeField] private GameDataEventsDispatchTester _eventsDispatchTester;
        [SerializeField] private GameDataEventsCSVSaverConfig _csvSaverConfig;
        
        private GameDataEventsListener _eventsListener;
        private GameDataEventsCSVSaver _gameDataEventsCSVSaver;

        public void Install(IEventSystemService eventSystemService)
        {
            IActiveSceneDataEventsProvider activeSceneDataEventsProvider = new TestingActiveSceneDataEventsProvider();
            
            _gameDataEventsCSVSaver = 
                new GameDataEventsCSVSaver(_csvSaverConfig);
                
            _eventsListener = 
                new GameDataEventsListener(eventSystemService, _gameDataEventsCSVSaver, activeSceneDataEventsProvider);
            
            _eventsListener.StartListening();
            
            _eventsDispatchTester.Init(eventSystemService);
        }
        
        public void Uninstall()
        {
            _eventsListener.StopListening();
            _gameDataEventsCSVSaver.Finish();
        }
    }
}