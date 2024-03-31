using Popeye.Modules.WorldElements.WorldInteractors;
using Popeye.Scripts.EventChannels;
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