using Popeye.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public struct OnPlayerHealEvent
    {
        public Vector3 Position { get; private set; }
        public int CurrentHealth { get; private set; }
        public int HealthBeforeHealing { get; private set; }

        public OnPlayerHealEvent(Vector3 position, int currentHealth, int healthBeforeHealing)
        {
            Position = position;
            CurrentHealth = currentHealth;
            HealthBeforeHealing = healthBeforeHealing;
        }
    }
    
    public class PlayerHealEventData
    {
        public const string NAME = "Player Heal";
        public GenericEventData GenericEventData { get; private set; }
        public Vector3 Position { get; private set; }
        public int CurrentHealth { get; private set; }
        public int HealthBeforeHealing { get; private set; }

        public PlayerHealEventData(GenericEventData genericEventData, OnPlayerHealEvent eventInfo)
        {
            GenericEventData = genericEventData;
            Position = eventInfo.Position;
            CurrentHealth = eventInfo.CurrentHealth;
            HealthBeforeHealing = eventInfo.HealthBeforeHealing;
        }
    }
    
    
}