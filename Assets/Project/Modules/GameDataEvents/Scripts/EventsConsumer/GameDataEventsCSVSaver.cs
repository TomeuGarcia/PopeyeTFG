using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Popeye.Modules.GameDataEvents
{
    public class GameDataEventsCSVSaver : IGameDataEventsConsumer
    {
        private readonly GameDataEventsCSVSaverConfig _config;
        private readonly List<string> _dataToSave;
        private StreamWriter _outWriter;

        public GameDataEventsCSVSaver(GameDataEventsCSVSaverConfig config)
        {
            _config = config;
            _dataToSave = new List<string>(10);

        }

        private bool DataFileExists()
        {
            return File.Exists(_config.FilePathWithExtention);
        }
        public void Finish()
        {
            OpenFile();
            SaveData();
            CloseFile();
        }

        private void OpenFile()
        {
            if(DataFileExists())
            {
                File.Delete(_config.FilePathWithExtention);
            }

            _outWriter = File.CreateText(_config.FilePathWithExtention);

        }
        private void CloseFile()
        {
            _outWriter.Close();
        }

        private void SaveData() //Saves all data stored in _dataToSave at once
        {
            foreach(string dataRow in _dataToSave) 
            { 
                _outWriter.WriteLine(dataRow);
            }
        }


        public void AddEventContent(string eventContent)
        {
            _dataToSave.Add(eventContent);

            if (_config.LogToConsole)
            {
                Debug.Log(eventContent);
            }
        }
        
        
    }
}