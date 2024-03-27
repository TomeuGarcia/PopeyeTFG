using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies.General;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public struct OnEnemyTakeDamageEvent
    {
        public EnemyID Id { get; private set; }
        public Vector3 Position { get; private set; }
        public DamageHit DamageHit { get; private set; }

        public OnEnemyTakeDamageEvent(EnemyID id, Vector3 position, DamageHit damageHit)
        {
            Id = id;
            Position = position;
            DamageHit = damageHit;
        }
    }
    
    public class EnemyTakeDamageEventData
    {
        public const string NAME = "Enemy Take Damage";
        public GenericEventData GenericEventData { get; private set; }
        public string EnemyName { get; private set; }
        public Vector3 Position { get; private set; }
        public string DamageHitName { get; private set; }


        public EnemyTakeDamageEventData(GenericEventData genericEventData, OnEnemyTakeDamageEvent eventInfo)
        {
            GenericEventData = genericEventData;
            EnemyName = eventInfo.Id.GetEnemyName();
            Position = eventInfo.Position;
            DamageHitName = eventInfo.DamageHit.GetName();
        }
    }
    
    
}