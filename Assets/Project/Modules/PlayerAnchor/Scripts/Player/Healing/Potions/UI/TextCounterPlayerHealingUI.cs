using TMPro;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class TextCounterPlayerHealingUI : MonoBehaviour, IPlayerHealingUI
    {
        [SerializeField] private TextMeshProUGUI _maxNumberOfHealsText;
        [SerializeField] private TextMeshProUGUI _currentNumberOfHealsText;
        
        
        public void Setup(int maxNumberOfHeals, int currentNumberOfHeals)
        {
            SetMaxNumberOfHealsText(maxNumberOfHeals);
            SetCurrentNumberOfHealsText(currentNumberOfHeals);
        }

        public void OnHealUsed(int currentNumberOfHeals)
        {
            SetCurrentNumberOfHealsText(currentNumberOfHeals);
        }

        public void OnHealsExhausted()
        {
            
        }

        public void OnHealsReset(int maxNumberOfHeals)
        {
            SetCurrentNumberOfHealsText(maxNumberOfHeals);
        }
        

        private void SetMaxNumberOfHealsText(int number)
        {
            _maxNumberOfHealsText.text = number.ToString();
        }
        private void SetCurrentNumberOfHealsText(int number)
        {
            _currentNumberOfHealsText.text = number.ToString();
        }
        
    }
}