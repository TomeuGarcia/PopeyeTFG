using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Cysharp.Threading.Tasks;

namespace Popeye.Modules.GameDataEvents
{
    public class GameDataEventsCSVSaver : IGameDataEventsConsumer
    {
        private readonly GameDataEventsCSVSaverConfig _config;
        private readonly List<string> _dataToSave;
        private StreamWriter _outWriter;
        private bool _savingData;

        public GameDataEventsCSVSaver(GameDataEventsCSVSaverConfig config)
        {
            _config = config;
            _dataToSave = new List<string>(300);
            _savingData = false;
        }

        public void Start()
        {
            SaveOverTime().Forget();
        }
        
        public void Finish()
        {
            WaitUntilDataIsSaved();
            SaveCurrentData();
        }

        private void WaitUntilDataIsSaved()
        {
            while (_savingData) { }
        }

        private async UniTaskVoid SaveOverTime()
        {
            await UniTask.Delay(_config.SaveFrequency);
            SaveCurrentData();
        }


        private void SaveCurrentData()
        {
            WaitUntilDataIsSaved();
            
            _savingData = true;
            OpenFile();
            SaveDataToFile();
            CloseFile();
            _savingData = false;
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

        private void SaveDataToFile()
        {
            string[] dataToSaveCopy = _dataToSave.ToArray();
            _dataToSave.Clear();
            
            foreach(string dataRow in dataToSaveCopy) 
            { 
                _outWriter.WriteLine(dataRow);
            }
        }


        public void AddEventContent(string eventContent)
        {

#if UNITY_EDITOR
            if (!_config.SaveInEditor) return;
#endif
            
            WaitUntilDataIsSaved();
            _dataToSave.Add(eventContent);

            if (_config.LogToConsole)
            {
                Debug.Log(eventContent);
            }
        }
        
        
    }
}