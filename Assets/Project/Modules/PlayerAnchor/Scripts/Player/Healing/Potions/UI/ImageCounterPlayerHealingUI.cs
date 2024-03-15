using DG.Tweening;
using Project.Scripts.TweenExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class ImageCounterPlayerHealingUI : MonoBehaviour, IPlayerHealingUI
    {
        [Header("TEXTS")]
        [SerializeField] private TextMeshProUGUI[] _texts;
        [SerializeField] private TextMeshProUGUI _currentNumberOfHealsText;
        [SerializeField] private RectTransform _textsHolder;
        
        [Header("IMAGE")]
        [SerializeField] private Image _healItemImage;
        [SerializeField] private RectTransform _healItemHolder;

        [Header("CONFIGURATION")]
        [SerializeField] private PlayerHealingUIConfig _config;
        
        
        private Color _healItemNormalColor;
        private Color _healItemExhaustedColor;

        private Color _currentHealsTextColor;
        
        
        
        public void Setup(int maxNumberOfHeals, int currentNumberOfHeals)
        {
            SetCurrentNumberOfHealsText(currentNumberOfHeals);
            _healItemNormalColor = _healItemImage.color;
            _healItemExhaustedColor = _healItemImage.color * _config.HealsExhaustedTextColor;
        }

        public void OnHealUsed(int currentNumberOfHeals)
        {
            SetCurrentNumberOfHealsText(currentNumberOfHeals);

            _textsHolder.PunchScale(_config.TextPunch);
            _healItemHolder.PunchScale(_config.ImagePunch);
            
            foreach (var text in _texts)
            {
                text.PunchColor(_config.TextColorPunch);
            }
        }

        public void OnHealsExhausted()
        {
            _healItemImage.color = _healItemExhaustedColor;

            foreach (var text in _texts)
            {
                text.color = _config.HealsExhaustedTextColor;
            }
            _currentHealsTextColor = _config.HealsExhaustedTextColor;
        }

        public void OnHealsReset(int maxNumberOfHeals)
        {
            SetCurrentNumberOfHealsText(maxNumberOfHeals);
            _healItemImage.color = _healItemNormalColor;
            
            foreach (var text in _texts)
            {
                text.color = _config.HealsReadyTextColor;
            }
            _currentHealsTextColor = _config.HealsReadyTextColor;
        }
        

        private void SetCurrentNumberOfHealsText(int number)
        {
            _currentNumberOfHealsText.text = number.ToString();
        }
    }
}