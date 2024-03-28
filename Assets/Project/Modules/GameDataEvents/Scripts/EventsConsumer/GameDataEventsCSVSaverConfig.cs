using NaughtyAttributes;
using Popeye.ProjectHelpers;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
        
        private string DirectoryPathBuild =>  Application.streamingAssetsPath;
        private string DirectoryPathProject =>  Application.dataPath;
        public string DirectoryPath =>  DirectoryPathBuild + _filePath;
        public string FilePath =>  DirectoryPath + _fileName;
        public string FilePathWithExtension => FilePath + _fileExtension;

        
#if UNITY_EDITOR
        [Button()]
        private void ShowInFolder()
        {
            EditorUtility.RevealInFinder(DirectoryPath);
        }
#endif
        
    }
}