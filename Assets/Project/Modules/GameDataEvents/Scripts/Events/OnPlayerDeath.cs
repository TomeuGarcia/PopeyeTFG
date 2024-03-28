using Popeye.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public struct OnPlayerDeathEvent
    {
        public Vector3 Position { get; private set; }
        public DamageHit DamageHit { get; private set; }

        public OnPlayerDeathEvent(Vector3 position, DamageHit damageHit)
        {
            Position = position;
            DamageHit = damageHit;
        }
    }
    
    public class PlayerDeathEventData
    {
        public const string NAME = "Player Death";
        public GenericEventData GenericEventData { get; private set; }
        public Vector3 Position { get; private set; }
        public string DamageHitName { get; private set; }

        public PlayerDeathEventData(GenericEventData genericEventData, OnPlayerDeathEvent eventInfo)
        {
            GenericEventData = genericEventData;
            Position = eventInfo.Position;
            DamageHitName = eventInfo.DamageHit.GetName();
        }
    }
    
    
}