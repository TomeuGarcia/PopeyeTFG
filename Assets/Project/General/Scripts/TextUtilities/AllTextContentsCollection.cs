using NaughtyAttributes;
using Popeye.ProjectHelpers;
using Popeye.Scripts.TextUtilities;
using UnityEngine;

namespace Popeye.Scripts.Core.Scenes
{
    [CreateAssetMenu(fileName = "AllTextContentsCollection", 
        menuName = ScriptableObjectsHelper.TEXTUTILITIES_ASSETS_PATH + "AllTextContentsCollection")]
    public class AllTextContentsCollection : ScriptableObject
    {
        [Expandable] [SerializeField] private TextContent[] _incompleteTextContents;
        [Expandable] [SerializeField] private TextContent[] _allTextContents;
        
        
        public void UpdateTextContents(TextContent[] allTextContents, TextContent[] incompleteTextContents)
        {
            _allTextContents = allTextContents;           
            _incompleteTextContents = incompleteTextContents;           
        }
        
        
    }
}