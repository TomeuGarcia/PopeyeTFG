using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    [CreateAssetMenu(fileName = "GameDataEvents_ExcelSaverConfig", 
        menuName = ScriptableObjectsHelper.GAMEDATAEVENTS_ASSETS_PATH + "ExcelSaverConfig")]
    public class GameDataEventsExcelSaverConfig : ScriptableObject
    {
        [SerializeField] private string _filePath;
        [SerializeField] private string _fileName;

        public string FilePath => _filePath + "/" + _fileName;
    }
}