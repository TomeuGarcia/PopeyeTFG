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
        
        
        [Header("DAMAGED")]
        [SerializeField] private List<MaterialFlash> _flashSequence = new();
        public List<MaterialFlash> FlashSequence => _flashSequence;

        [Header("TIRED")]
        [SerializeField] private string _isTiredProperty;
        [SerializeField] private float _tiredTransitionTime;
        public string IsTiredProperty => _isTiredProperty;
        public float TiredTransitionTime => _tiredTransitionTime;
        
        [Header("DASH")]
        [SerializeField] private string _dashingProperty;
        [SerializeField] private float _dashMaterialTransitionTime;
        public string DashingProperty => _dashingProperty;
        public float DashMaterialTransitionTime => _dashMaterialTransitionTime;
        
    }
}