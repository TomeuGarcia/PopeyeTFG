using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Scripts.TextUtilities
{
    [CreateAssetMenu(fileName = "TextContent_NAME",
       menuName = ScriptableObjectsHelper.TEXTUTILITIES_ASSETS_PATH + "TextContent")]
    public class TextContent : ScriptableObject
    {
        [TextArea][SerializeField] private string _content_ENG;
        [TextArea][SerializeField] private string _content_CAT;
        [TextArea][SerializeField] private string _content_ESP;
        public string Content => _content_ENG;

        public bool HasAllFieldsCompleted()
        {
            return IsFieldCompleted(ref _content_ENG) &&
                   IsFieldCompleted(ref _content_CAT) &&
                   IsFieldCompleted(ref _content_ESP);
        }

        private bool IsFieldCompleted(ref string content)
        {
            return content != "";
        }
    }
}


