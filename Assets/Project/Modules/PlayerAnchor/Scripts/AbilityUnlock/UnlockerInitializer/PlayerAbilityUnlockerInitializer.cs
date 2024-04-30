using System;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
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

        [Header("VIEW")] 
        [SerializeField] private AbilityUnlockerChristalView _christalView;
        [SerializeField] private AbilityUnlockerChainedOrbView _orbView;
        
        [Header("SOUNDS")]
        [SerializeField] private AbilityUnlockerChristalAudio _christalAudio;
        
        
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
            
            _christalView?.Configure(ServiceLocator.Instance.GetService<IGameReferences>(), _christalAudio);
            _orbView?.Configure(ServiceLocator.Instance.GetService<IGameReferences>(), _christalAudio, _abilityToUnlock);

            IPlayerAbilityUnlockerView view = _christalView != null ? _christalView : _orbView;
            _playerAbilityUnlocker.Configure(configureReferences.AbilityChannel, view);

            
            ITutorialDisplayCondition tutorialStopDisplayCondition = null;
            if (configureReferences.StopShowingCondition == ITutorialDisplayCondition.Type.TimesPerformed)
            {
                tutorialStopDisplayCondition = new PerformedAmountDisplayCondition(
                    configureReferences.TutorialHideChannel,
                    configureReferences.TimesToStopShowing
                );
            }
            else if (configureReferences.StopShowingCondition == ITutorialDisplayCondition.Type.Duration)
            {
                tutorialStopDisplayCondition = new DurationDisplayCondition(
                    configureReferences.DurationToStopShowing
                );
            }
            
            _tutorialInformationDisplay.Configure(
                configureReferences.TextInfoToDisplay,
                configureReferences.VideoInfoToDisplay,
                tutorialStopDisplayCondition);
        }
        
    }
}