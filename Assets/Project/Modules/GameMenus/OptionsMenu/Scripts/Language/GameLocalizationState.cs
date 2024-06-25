using System;
using System.Linq;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using UnityEngine;

namespace Popeye.Modules.GameMenus.OptionsMenu
{
    public static class GameLocalizationState
    {
        public enum Language
        {
            English,
            Catalan,
            Spanish
        }

        public struct OnGameLanguageUpdatedEvent
        {
            public OnGameLanguageUpdatedEvent(Language language)
            {
                Language = language;
            }
            public Language Language { get; private set; }
        }
        
        
        public static Language GameLanguage {get; private set; } = Language.English;
        public static readonly Language[] AllGameLanguages = Enum.GetValues(typeof(Language)) as Language[];

        public static void InitWithSystemLanguage()
        {
            Language gameLanguage = Language.English;
            
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    gameLanguage = Language.English;
                    break;
                case SystemLanguage.Catalan:
                    gameLanguage = Language.Catalan;
                    break;
                case SystemLanguage.Spanish:
                    gameLanguage = Language.Spanish;
                    break;
            }

            UpdateGameLanguage(gameLanguage);
        }

        public static void UpdateGameLanguage(int languageIndex)
        {
            UpdateGameLanguage(AllGameLanguages[languageIndex]);
        }
        private static void UpdateGameLanguage(Language gameLanguage)
        {
            GameLanguage = gameLanguage;

            if (ServiceLocator.Instance.Contains<IEventSystemService>())
            {
                ServiceLocator.Instance.GetService<IEventSystemService>()
                    .Dispatch(new GameLocalizationState.OnGameLanguageUpdatedEvent(GameLanguage));
            }
        }

        public static int GetGameLanguageIndex()
        {
            return (int)GameLanguage;
        }
        
    }
}