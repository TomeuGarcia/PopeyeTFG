using Popeye.Modules.GameMenus.Generic;

namespace Popeye.Modules.GameMenus.OptionsMenu
{
    public class LanguageOptionController : IOptionSelectorListener
    {
        public void OnOptionUpdated(int optionIndex)
        {
            GameLocalizationState.UpdateGameLanguage(optionIndex);
        }   
    }
}