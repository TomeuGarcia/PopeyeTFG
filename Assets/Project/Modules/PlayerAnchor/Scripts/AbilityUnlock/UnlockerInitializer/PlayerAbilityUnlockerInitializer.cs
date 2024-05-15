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

        private void Awake()
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
                tutorialStopDisplayCondition = CreateTimesPerformedCondition(configureReferences);
            }
            else if (configureReferences.StopShowingCondition == ITutorialDisplayCondition.Type.Duration)
            {
                tutorialStopDisplayCondition = CreateDurationCondition(configureReferences);
            }
            else if (configureReferences.StopShowingCondition == ITutorialDisplayCondition.Type.TimesPerformedAndDuration)
            {
                tutorialStopDisplayCondition = CreateTimesPerformedAndDurationCondition(configureReferences);
            }
            
            _tutorialInformationDisplay.Configure(
                configureReferences.TextInfoToDisplay,
                configureReferences.VideoInfoToDisplay,
                tutorialStopDisplayCondition);
        }


        private ITutorialDisplayCondition CreateTimesPerformedCondition(
            GeneralInitializePlayerAbilityUnlockerConfig.References configureReferences)
        {
            return new PerformedAmountDisplayCondition(
                configureReferences.TutorialHideChannel,
                configureReferences.TimesToStopShowing
            );
        }
        
        private ITutorialDisplayCondition CreateDurationCondition(
            GeneralInitializePlayerAbilityUnlockerConfig.References configureReferences)
        {
            return new DurationDisplayCondition(
                configureReferences.DurationToStopShowing
            );
        }
        
        private ITutorialDisplayCondition CreateTimesPerformedAndDurationCondition(
            GeneralInitializePlayerAbilityUnlockerConfig.References configureReferences)
        {
            return new ComposedTutorialDisplayCondition(
                new []
                {
                    CreateTimesPerformedCondition(configureReferences), 
                    CreateDurationCondition(configureReferences)
                }
            );
        }
        
    }
}