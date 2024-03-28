using System;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public class GameDataEventsCSVSaver : IGameDataEventsConsumer
    {
        private GameDataEventsCSVSaverConfig _config;

        public GameDataEventsCSVSaver(GameDataEventsCSVSaverConfig config)
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


        public void AddEventContent(string eventContent)
        {
            Debug.Log(eventContent);
            SaveData("TODO");
        }
    }
}