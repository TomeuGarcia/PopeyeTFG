using Popeye.Core.Services.EventSystem;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public class GameDataEventsInstaller : MonoBehaviour
    {
        [SerializeField] private GameDataEventsDispatchTester _eventsDispatchTester;
        [SerializeField] private GameDataEventsExcelSaverConfig _excelSaverConfig;
        
        private GameDataEventsListener _eventsListener;
        private GameDataEventsExcelSaver _gameDataEventsExcelSaver;

        public void Install(IEventSystemService eventSystemService)
        {
            IActiveSceneDataEventsProvider activeSceneDataEventsProvider = new TestingActiveSceneDataEventsProvider();
            
            _gameDataEventsExcelSaver = 
                new GameDataEventsExcelSaver(_excelSaverConfig);
                
            _eventsListener = 
                new GameDataEventsListener(eventSystemService, _gameDataEventsExcelSaver, activeSceneDataEventsProvider);
            
            _eventsListener.StartListening();
            
            _eventsDispatchTester.Init(eventSystemService);
        }
        
        public void Uninstall()
        {
            _eventsListener.StopListening();
            _gameDataEventsExcelSaver.Finish();
        }
    }
}