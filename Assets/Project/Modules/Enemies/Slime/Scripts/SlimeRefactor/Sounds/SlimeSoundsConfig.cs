using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Popeye.Modules.AudioSystem;
using Popeye.Modules.Enemies.Slime;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Modules.Enemies.Slime.Scripts.SlimeRefactor
{
    [CreateAssetMenu(fileName = "SlimeSoundsConfig", 
        menuName = ScriptableObjectsHelper.ENEMIES_ASSET_PATH + "SlimeSoundsConfig")]
    public class SlimeSoundsConfig : ScriptableObject
    {
        [Expandable] [SerializeField] private OneShotFMODSound _divide;
        [Expandable] [SerializeField] private OneShotFMODSound _death;


        [System.Serializable]
        struct SlimeSizeToSoundParameter
        {
            [SerializeField] private SlimeSizeID _sizeID;
            [SerializeField] private float _parameterValue;
            
            public SlimeSizeID SizeID => _sizeID;
            public float ParameterValue => _parameterValue;
        }

        [SerializeField] private SoundParameter _slimeSizeParameter;
        [SerializeField] private SlimeSizeToSoundParameter[] _sizeToParameterValue;
        private Dictionary<SlimeSizeID, float> _sizeToParameterValueDictionary;


        private void OnEnable()
        {
            _sizeToParameterValueDictionary = new Dictionary<SlimeSizeID, float>(_sizeToParameterValue.Length);
            foreach (var slimeSizeToSoundParameter in _sizeToParameterValue)
            {
                _sizeToParameterValueDictionary.Add(slimeSizeToSoundParameter.SizeID, slimeSizeToSoundParameter.ParameterValue);
            }
        }


        public void PlayDivideSound(IFMODAudioManager audioManager, GameObject attachedGameObject, SlimeSizeID sizeID)
        {
            PlayOneShotSound(audioManager, attachedGameObject, sizeID, _divide);
        }
        public void PlayDeathSound(IFMODAudioManager audioManager, GameObject attachedGameObject, SlimeSizeID sizeID)
        {
            PlayOneShotSound(audioManager, attachedGameObject, sizeID, _death);
        }

        private void PlayOneShotSound(IFMODAudioManager audioManager, GameObject attachedGameObject, 
            SlimeSizeID sizeID, OneShotFMODSound sound)
        {
            _slimeSizeParameter.Value = _sizeToParameterValueDictionary[sizeID];
            audioManager.PlayOneShotAttached(sound, attachedGameObject);
        }
        
        
    }
}