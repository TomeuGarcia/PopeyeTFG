using System.Collections.Generic;
using Popeye.Modules.VFX.Generic;
using Popeye.Modules.VFX.Generic.MaterialInterpolationConfiguration;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerAnchor.Player
{
    [System.Serializable]
    public class PlayerMaterialViewConfig
    {
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _damagedColor;
        [SerializeField] private Color _healColor;
        public Color NormalColor => _normalColor;
        public Color DamagedColor => _damagedColor;
        public Color HealColor => _healColor;
        
        
        [SerializeField] private List<MaterialFlash> _flashSequence = new();
        public List<MaterialFlash> FlashSequence => _flashSequence;

        [SerializeField] private string _isTiredProperty;
        [SerializeField] private float _tiredTransitionTime;
        public string IsTiredProperty => _isTiredProperty;
        public float TiredTransitionTime => _tiredTransitionTime;
        

        [SerializeField] private PlayerMaterialView.FlickData _takeDamageFlick;
        [SerializeField] private PlayerMaterialView.FlickData _deathFlick;
        [SerializeField] private PlayerMaterialView.FlickData _healFlick;
        public PlayerMaterialView.FlickData TakeDamageFlick => _takeDamageFlick;
        public PlayerMaterialView.FlickData DeathFlick => _deathFlick;
        public PlayerMaterialView.FlickData HealFlick => _healFlick;
    }
}