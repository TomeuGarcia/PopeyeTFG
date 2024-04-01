using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies.General;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public struct OnEnemyTakeDamageEvent
    {
        public EnemyID Id { get; private set; }
        public Vector3 Position { get; private set; }
        public DamageHitResult DamageHitResult { get; private set; }

        public OnEnemyTakeDamageEvent(EnemyID id, Vector3 position, DamageHitResult damageHitResult)
        {
            Id = id;
            Position = position;
            DamageHitResult = damageHitResult;
        }
    }
    
    public class EnemyTakeDamageEventData
    {
        public const string NAME = "Enemy Take Damage";
        public GenericEventData GenericEventData { get; private set; }
        public string EnemyName { get; private set; }
        public Vector3 Position { get; private set; }
        
        public bool WasKilled { get; private set; }
        public string DamageHitName { get; private set; }


        public EnemyTakeDamageEventData(GenericEventData genericEventData, OnEnemyTakeDamageEvent eventInfo)
        {
            GenericEventData = genericEventData;
            EnemyName = eventInfo.Id.GetEnemyName();
            Position = eventInfo.Position;
            if (eventInfo.DamageHitResult.DamageHitTarget != null)
            {
                WasKilled = eventInfo.DamageHitResult.DamageHitTarget.IsDead();
            }
            DamageHitName = eventInfo.DamageHitResult.DamageHit.GetName();
        }
    }
    
    
}