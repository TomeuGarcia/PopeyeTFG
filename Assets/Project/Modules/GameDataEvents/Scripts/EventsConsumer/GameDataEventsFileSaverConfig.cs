using System;
using NaughtyAttributes;
using Popeye.ProjectHelpers;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Popeye.Modules.GameDataEvents
{
    [CreateAssetMenu(fileName = "GameDataEvents_EXTENSIONSaverConfig", 
        menuName = ScriptableObjectsHelper.GAMEDATAEVENTS_ASSETS_PATH + "FileSaverConfig")]
    public class GameDataEventsFileSaverConfig : ScriptableObject
    {
        [Header("LOGGING")]
        [SerializeField] private bool _logToConsole = false;
        
        [Header("SAVE")]
        [SerializeField] private bool _saveInEditor = false; 
        [SerializeField, Range(0f, 10f)] private float _saveFrequencyInMinutes = 1f; 
        
        [Header("PATH")]
        [SerializeField] private string _filePath;
        [SerializeField] private string _fileName;
        [SerializeField] private string _fileExtension;

        public bool LogToConsole =>  _logToConsole;
        
        private string DirectoryPathBuild =>  Application.streamingAssetsPath;
        private string DirectoryPathProject =>  Application.dataPath;
        public string DirectoryPath =>  DirectoryPathBuild + _filePath;
        public string FilePath =>  DirectoryPath + _fileName;
        public string FilePathWithExtension => FilePath + _fileExtension;

        public bool SaveInEditor => _saveInEditor; 
        public TimeSpan SaveFrequency => TimeSpan.FromMinutes(_saveFrequencyInMinutes);

    }
}