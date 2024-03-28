using Popeye.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public struct OnPlayerTakeDamageEvent
    {
        public Vector3 Position { get; private set; }
        public DamageHit DamageHit { get; private set; }
        public int CurrentHealth { get; private set; }

        public OnPlayerTakeDamageEvent(Vector3 position, DamageHit damageHit, int currentHealth)
        {
            Position = position;
            DamageHit = damageHit;
            CurrentHealth = currentHealth;
        }
    }
    
    public class PlayerTakeDamageEventData
    {
        public const string NAME = "Player Take Damage";
        public GenericEventData GenericEventData { get; private set; }
        public Vector3 Position { get; private set; }
        public string DamageHitName { get; private set; }
        public int CurrentHealth { get; private set; }

        public PlayerTakeDamageEventData(GenericEventData genericEventData, OnPlayerTakeDamageEvent eventInfo)
        {
            GenericEventData = genericEventData;
            Position = eventInfo.Position;
            DamageHitName = eventInfo.DamageHit.GetName();
            CurrentHealth = eventInfo.CurrentHealth;
        }
    }
    
    
}