using System;
using System.Collections.Generic;
using Popeye.Core.Services.InformationDisplay;
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
            SpecialAttack
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
            
            [Header("TUTORIAL")]
            [SerializeField] private EmptyEventChannelAsset _tutorialHideChannel;
            [SerializeField] private TextDisplayConfig _tutorialInfoToDisplay;
            [SerializeField, Range(1, 10)] private int _timesToStopShowing = 1;
            
            public EmptyEventChannelAsset AbilityChannel => _abilityToUnlockChannel;
            public EmptyEventChannelAsset TutorialHideChannel => _tutorialHideChannel;
            public TextDisplayConfig TutorialInfoToDisplay => _tutorialInfoToDisplay;
            public int TimesToStopShowing => _timesToStopShowing;
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