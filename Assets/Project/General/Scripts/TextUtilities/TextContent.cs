using System;
using Popeye.Modules.GameMenus.OptionsMenu;
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
        public string Content => GetContentByLanguage();

        public static Action<TextContent> OnContentUpdated;

        private void OnValidate()
        {
            OnContentUpdated?.Invoke(this);
        }


        private string GetContentByLanguage()
        {
            switch (GameLocalizationState.GameLanguage)
            {
                case GameLocalizationState.Language.English:
                    return _content_ENG;
                case GameLocalizationState.Language.Catalan:
                    return _content_CAT;
                case GameLocalizationState.Language.Spanish:
                    return _content_ESP;
            }

            return _content_ENG;
        }
        

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

        public void GetContentsByLanguage(out string content_ENG, out string content_CAT, out string content_ESP)
        {
            content_ENG = _content_ENG;
            content_CAT = _content_CAT;
            content_ESP = _content_ESP;
        }
    }
}


