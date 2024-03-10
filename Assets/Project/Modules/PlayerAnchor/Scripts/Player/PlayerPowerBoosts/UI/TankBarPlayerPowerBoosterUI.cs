using Popeye.Modules.ValueStatSystem;
using TMPro;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts
{
    public class TankBarPlayerPowerBoosterUI : MonoBehaviour, IPlayerPowerBoosterUI
    {
        [SerializeField] private GameObject _nextLevelNumberHolder;
        [SerializeField] private TextMeshProUGUI _nextLevelNumberText;
        [SerializeField] private ValueStatBar _tankBar;

        private int _currentNextLevelNumber;

        
        public void Init(AValueStat experienceValueStat)
        {
            _tankBar.Init(experienceValueStat);

            _currentNextLevelNumber = 0;
            HideText();
        }

        public void OnLevelAdded(int nextLevelNumber)
        {
            UpdateText(nextLevelNumber);
        }

        public void OnLevelLost(int nextLevelNumber)
        {
            UpdateText(nextLevelNumber);
        }


        private void UpdateText(int nextLevelNumber)
        {
            if (_currentNextLevelNumber < 2)
            {                
                _nextLevelNumberText.text = nextLevelNumber.ToString();
                ShowText();
            }
            else 
            {
                if (nextLevelNumber < 2)
                {
                    HideText();
                }
                else
                {
                    _nextLevelNumberText.text = nextLevelNumber.ToString();
                }
            }
            
            _currentNextLevelNumber = nextLevelNumber;
        }
        private void ShowText()
        {
            _nextLevelNumberHolder.gameObject.SetActive(true);
        }
        private void HideText()
        {
            _nextLevelNumberHolder.gameObject.SetActive(false);
        }
        
        
    }
}