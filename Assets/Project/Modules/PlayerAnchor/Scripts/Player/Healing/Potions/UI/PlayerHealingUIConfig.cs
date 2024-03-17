using DG.Tweening;
using Popeye.ProjectHelpers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    [CreateAssetMenu(fileName = "PlayerHealingUIConfig", 
        menuName = ScriptableObjectsHelper.PLAYERUI_ASSETS_PATH + "PlayerHealingUIConfig")]
    public class PlayerHealingUIConfig : ScriptableObject
    {
        [Header("TEXT COLORS")]
        [SerializeField] private Color _healsReadyTextColor = Color.white;
        [SerializeField] private Color _healsExhaustedTextColor = Color.gray;
        [SerializeField] private TweenColorConfigAsset _textColorPunch;
        public TweenColorConfigAsset TextColorPunch => _textColorPunch;
        
        
        [Header("TEXTS PUNCHING")]
        [SerializeField] private TweenPunchConfigAsset _textPunch;
        public TweenPunchConfigAsset TextPunch => _textPunch;
        
        [Header("IMAGE PUNCHING")]
        [SerializeField] private TweenPunchConfigAsset _imagePunch;
        public TweenPunchConfigAsset ImagePunch => _imagePunch;
        
        
        public Color HealsReadyTextColor => _healsReadyTextColor;
        public Color HealsExhaustedTextColor => _healsExhaustedTextColor;

        
    }
}