using System;
using FMOD.Studio;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    [CreateAssetMenu(fileName = "SoundParameter_NAME", 
        menuName = ScriptableObjectsHelper.SOUNDSYSTEM_ASSETS_PATH + "SoundParameter")]
    public class SoundParameter : ScriptableObject
    {
        [SerializeField] [FMODUnity.ParamRefAttribute] private string _name;
        [SerializeField] private float _value = 1.0f;

        private bool _wasInitialized = false;
        private FMOD.Studio.PARAMETER_DESCRIPTION _parameterDescription;
        
        public PARAMETER_ID Id
        {
            get {
                if (!_wasInitialized)
                {
                    Init();
                }
                return _parameterDescription.id;
            }
        }

        public float Value
        {
            get => _value;
            set => _value = value;
        }
    

        public delegate void Event(SoundParameter parameter);
        public Event OnValueChanged;


        private void OnEnable()
        {
            _wasInitialized = false;
        }

        private void OnValidate()
        {
            InvokeOnValueChanged();
        }

        private void Init()
        {
            FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName(_name, out _parameterDescription);
        }

        private void InvokeOnValueChanged()
        {
            OnValueChanged?.Invoke(this);
        }

        
        public void SetValue(float value)
        {
            _value = value;
            InvokeOnValueChanged();
        }
        
    }
}