using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    [CreateAssetMenu(fileName = "GameDataEvents_CSVSaverConfig", 
        menuName = ScriptableObjectsHelper.GAMEDATAEVENTS_ASSETS_PATH + "CSVSaverConfig")]
    public class GameDataEventsCSVSaverConfig : ScriptableObject
    {
        [SerializeField] private string _filePath;
        [SerializeField] private string _fileName;

        public string FilePath =>  Application.dataPath + _filePath + _fileName;
    }
}