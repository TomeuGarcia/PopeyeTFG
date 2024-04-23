using System.Collections.Generic;
using DG.Tweening;
using Popeye.Modules.VFX.Generic;
using Popeye.Modules.VFX.Generic.MaterialInterpolationConfiguration;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerAnchor.Player
{
    [System.Serializable]
    public class PlayerMaterialViewConfig
    {
        [Header("HEAL")]
        [SerializeField] private string _healProperty;
        [SerializeField] private float _healAppearTime;
        [SerializeField] private Ease _healAppearEase;
        [SerializeField] private float _healDisappearTime;
        [SerializeField] private Ease _healDisappearEase;
        
        public string HealProperty => _healProperty;
        public float HealAppearTime => _healAppearTime;
        public Ease HealAppearEase => _healAppearEase;
        public float HealDisappearTime => _healDisappearTime;
        public Ease HealDisappearEase => _healDisappearEase;

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