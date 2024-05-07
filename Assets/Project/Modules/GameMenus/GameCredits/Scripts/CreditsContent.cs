using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Modules.GameMenus.GameCredits
{
    [CreateAssetMenu(fileName = "CreditsContent", 
        menuName = ScriptableObjectsHelper.GAMEMENU_ASSETS_PATH + "CreditsContent")]
    public class CreditsContent : ScriptableObject
    {
        [SerializeField] private string _title = "Credits";
        [SerializeField] private float _heightGap = 10f;
        [SerializeField] private CreditsBlock[] _blocks;
        
        public string Title => _title;
        public float HeightGap => _heightGap;
        public CreditsBlock[] Blocks => _blocks;
    }
}