using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class ImageCounterPlayerHealingUI : MonoBehaviour, IPlayerHealingUI
    {
        [SerializeField] private TextMeshProUGUI _currentNumberOfHealsText;
        [SerializeField] private Image _healItemImage;
        private Color _healItemNormalColor;
        private Color _healItemExhaustedColor;
        
        
        public void Setup(int maxNumberOfHeals, int currentNumberOfHeals)
        {
            SetCurrentNumberOfHealsText(currentNumberOfHeals);
            _healItemNormalColor = _healItemImage.color;
            _healItemExhaustedColor = _healItemImage.color * Color.gray;
        }

        public void OnHealUsed(int currentNumberOfHeals)
        {
            SetCurrentNumberOfHealsText(currentNumberOfHeals);
        }

        public void OnHealsExhausted()
        {
            _healItemImage.color = _healItemExhaustedColor;
        }

        public void OnHealsReset(int maxNumberOfHeals)
        {
            SetCurrentNumberOfHealsText(maxNumberOfHeals);
            _healItemImage.color = _healItemNormalColor;
        }
        

        private void SetCurrentNumberOfHealsText(int number)
        {
            _currentNumberOfHealsText.text = number.ToString();
        }
    }
}