using System;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public class GameDataEventsExcelSaver : IGameDataEventsConsumer
    {
        private GameDataEventsExcelSaverConfig _config;

        public GameDataEventsExcelSaver(GameDataEventsExcelSaverConfig config)
        {
            _config = config;
            OpenFile();
        }


        public void Finish()
        {
            CloseFile();
        }

        private void OpenFile()
        {
            // TODO
            //_config.FilePath;
        }
        private void CloseFile()
        {
            // TODO
        }
        private void SaveData(string data)
        {
            // TODO
        }
        

        public void AddEnemySeesPlayerEvent(EnemySeesPlayerEventData eventData)
        {
            // TODO save to file
            SaveData("TODO");
        }


    }
}