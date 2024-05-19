using Popeye.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public struct OnPlayerKilledByDamageEvent
    {
        public Vector3 Position { get; private set; }
        public DamageHitResult DamageHitResult { get; private set; }

        public OnPlayerKilledByDamageEvent(Vector3 position, DamageHitResult damageHitResult)
        {
            Position = position;
            DamageHitResult = damageHitResult;
        }
    }
    
    public class PlayerKilledByDamageEventData
    {
        public const string NAME = "Player Killed By Damage";
        public GenericEventData GenericEventData { get; private set; }
        public Vector3 Position { get; private set; }
        public string DamageHitName { get; private set; }

        public PlayerKilledByDamageEventData(GenericEventData genericEventData, OnPlayerKilledByDamageEvent eventInfo)
        {
            GenericEventData = genericEventData;
            Position = eventInfo.Position;
            DamageHitName = eventInfo.DamageHitResult.DamageHit.GetName();
        }
    }
}