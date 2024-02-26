using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    [CreateAssetMenu(fileName = "LastingSoundParameter_NAME", 
        menuName = ScriptableObjectsHelper.SOUNDSYSTEM_ASSETS_PATH + "LastingSoundParameter")]
    public class SoundParameter
    {
        [SerializeField] private string _name = "PARAMETER_NAME";
        [SerializeField] private float _value = 1.0f;
        
        public string Name => _name;
        public float Value => _value;


        public delegate void Event(SoundParameter parameter);
        public Event OnValueChanged;

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