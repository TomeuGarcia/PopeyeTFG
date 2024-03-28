using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    [CreateAssetMenu(fileName = "GameDataEvents_CSVSaverConfig", 
        menuName = ScriptableObjectsHelper.GAMEDATAEVENTS_ASSETS_PATH + "CSVSaverConfig")]
    public class GameDataEventsCSVSaverConfig : ScriptableObject
    {
        [SerializeField] private bool _logToConsole = false;
        [SerializeField] private string _filePath;
        [SerializeField] private string _fileName;
        [SerializeField] private string _fileExtension;

        public bool LogToConsole =>  _logToConsole;
        public string FilePath =>  Application.dataPath + _filePath + _fileName;
        public string FilePathWithExtention => FilePath + _fileExtension;
        
    }
}