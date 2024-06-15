using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Scripts.TextUtilities
{
    [CreateAssetMenu(fileName = "TextContent_NAME",
       menuName = ScriptableObjectsHelper.TEXTUTILITIES_ASSETS_PATH + "TextContent")]
    public class TextContent : ScriptableObject
    {

        [TextArea][SerializeField] private string _content_ENG;
        public string Content => _content_ENG;

    }
}


