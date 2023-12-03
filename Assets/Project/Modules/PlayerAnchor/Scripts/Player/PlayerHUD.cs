using Popeye.Modules.ValueStatSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private ValueStatBar _staminaBar;
        
        public void Configure(AValueStat staminaStat)
        {
            _staminaBar.Init(staminaStat);
        }
        
        
    }
}