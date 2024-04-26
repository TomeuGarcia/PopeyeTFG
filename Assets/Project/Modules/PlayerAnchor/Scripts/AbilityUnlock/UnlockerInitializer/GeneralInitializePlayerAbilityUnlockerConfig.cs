using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Popeye.Core.Services.InformationDisplay;
using Popeye.Modules.WorldElements.WorldInteractors;
using Popeye.ProjectHelpers;
using Popeye.Scripts.EventChannels;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    [CreateAssetMenu(fileName = "GeneralInitializePlayerAbilityUnlockerConfig", 
        menuName = ScriptableObjectsHelper.PLAYERABILITYUNLOCK_ASSETS_PATH + "GeneralInitializeConfig")]
    public class GeneralInitializePlayerAbilityUnlockerConfig : ScriptableObject
    {
        public enum Ability
        {
            None,
            Pull,
            DashDroppingAnchor,
            DashDroppingAnchorAttack,
            DashTowardsAnchor,
            SpecialAttack,
            
            HealthUpgrade
        }
        
        
        [System.Serializable]
        private struct AbilityChannelToReferences
        {
            [SerializeField] private Ability _ability;
            [SerializeField] private References _references;
            public Ability Ability => _ability;
            public References References => _references;

        }

        [System.Serializable]
        public class References
        {
            [Header("ABILITY")]
            [SerializeField] private EmptyEventChannelAsset _abilityToUnlockChannel;
            
            [Header("INFO TO DISPLAY")]
            [SerializeField] private TextDisplayConfig _tutorialInfoToDisplay;

            [Header("STOP SHOWING CONDITION")] 
            [SerializeField] private ITutorialDisplayCondition.Type _stopShowingCondition = ITutorialDisplayCondition.Type.TimesPerformed;
            [AllowNesting] [ShowIf("_stopShowingCondition", ITutorialDisplayCondition.Type.TimesPerformed)]
            [SerializeField, Range(1, 10)] private int _timesToStopShowing = 1;
            [AllowNesting] [ShowIf("_stopShowingCondition", ITutorialDisplayCondition.Type.TimesPerformed)]
            [SerializeField] private EmptyEventChannelAsset _tutorialHideChannel;
            
            [AllowNesting] [ShowIf("_stopShowingCondition", ITutorialDisplayCondition.Type.Duration)]
            [SerializeField, Range(0f, 10)] private float _durationToStopShowing = 5f;
            
            public EmptyEventChannelAsset AbilityChannel => _abilityToUnlockChannel;
            public EmptyEventChannelAsset TutorialHideChannel => _tutorialHideChannel;
            public TextDisplayConfig TutorialInfoToDisplay => _tutorialInfoToDisplay;
            public ITutorialDisplayCondition.Type StopShowingCondition => _stopShowingCondition;
            public int TimesToStopShowing => _timesToStopShowing;
            public float DurationToStopShowing => _durationToStopShowing;
        }
        
        
        
        [SerializeField] private AbilityChannelToReferences[] _abilityChannelToConfigReferences;

        public void GetReferences(Ability ability, out References references)
        {
            foreach (var it in _abilityChannelToConfigReferences)
            {
                if (it.Ability == ability)
                {
                    references = it.References;
                    return;
                }
            }
            
            references = null;
        }
    }
}