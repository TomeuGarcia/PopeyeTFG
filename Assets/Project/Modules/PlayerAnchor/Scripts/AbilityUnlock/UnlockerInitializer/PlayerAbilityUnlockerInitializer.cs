using System;
using Popeye.Modules.WorldElements.WorldInteractors;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public class PlayerAbilityUnlockerInitializer : MonoBehaviour
    {
        [Header("COMPONENTS")]
        [SerializeField] private PlayerAbilityUnlocker _playerAbilityUnlocker;
        [SerializeField] private TutorialInformationDisplay _tutorialInformationDisplay;
        [SerializeField] private GeneralInitializePlayerAbilityUnlockerConfig _initializeConfig;
        
        [Header("TYPE")]
        [SerializeField] private GeneralInitializePlayerAbilityUnlockerConfig.Ability _abilityToUnlock;

        private const string NAME_PREFIX = "SoulFragment_Group_";
        private const string NO_NAME_SUFFIX = "NAME";

        private void OnValidate()
        {
            string nameSuffix = _abilityToUnlock == GeneralInitializePlayerAbilityUnlockerConfig.Ability.None
                ? NO_NAME_SUFFIX
                : _abilityToUnlock.ToString();
            
            gameObject.name = NAME_PREFIX + nameSuffix;
        }

        private void Start()
        {
            _initializeConfig.GetReferences(_abilityToUnlock,
                out GeneralInitializePlayerAbilityUnlockerConfig.References configureReferences);
            
            
            _playerAbilityUnlocker.Configure(configureReferences.AbilityChannel);
            
            _tutorialInformationDisplay.Configure(configureReferences.TutorialInfoToDisplay,
                configureReferences.TutorialHideChannel, configureReferences.TimesToStopShowing);
        }
        
    }
}