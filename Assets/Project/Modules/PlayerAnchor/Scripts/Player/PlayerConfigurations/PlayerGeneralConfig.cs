using System;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations
{
    
    [CreateAssetMenu(fileName = "PlayerGeneralConfig", 
        menuName = PlayerConfigHelper.SO_ASSETS_PATH + "PlayerGeneralConfig")]
    public class PlayerGeneralConfig : ScriptableObject
    {
        [Header("Health")] 
        [SerializeField, Range(0, 300)] private int _maxHealth = 100;
        
        
        public int MaxHealth => _maxHealth;


    }
}