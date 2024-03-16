using System;
using Popeye.Modules.ValueStatSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    public class PlayerFocusUIEffects : MonoBehaviour
    {
        [SerializeField] private SmartImage _focusImage;
        [SerializeField] private string _canBeUsedPropertyName = "_CanBeUsed";
        private int _canBeUsedProperty;

        private void Awake()
        {
            _canBeUsedProperty = Shader.PropertyToID(_canBeUsedPropertyName);
        }

        public void SetCanBeUsed()
        {
            _focusImage.ImageMaterial.SetFloat(_canBeUsedProperty, 1.0f);
        }
        
        public void SetCanNotBeUsed()
        {
            _focusImage.ImageMaterial.SetFloat(_canBeUsedProperty, 0.0f);
        }
        
        
    }
}