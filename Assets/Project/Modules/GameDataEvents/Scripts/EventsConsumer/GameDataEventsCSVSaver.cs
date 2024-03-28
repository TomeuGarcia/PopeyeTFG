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
            _dataToSave = new List<string>(300);
        }
        
        public void Finish()
        {
            OpenFile();
            SaveData();
            CloseFile();
        }

        private void OpenFile()
        {
            string directoryPath = _config.DirectoryPath;
            string filePath = _config.FilePathWithExtension;

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            
            if (File.Exists(filePath))
            {
                _outWriter = new StreamWriter(filePath, true);
            }
            else
            {
                _outWriter = File.CreateText(filePath);
            }
        }
        
        private void CloseFile()
        {
            _outWriter.Close();
        }

        private void SaveData()
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