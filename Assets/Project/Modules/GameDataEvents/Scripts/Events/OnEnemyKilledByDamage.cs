using System.Collections.Generic;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies.General;
using Popeye.Modules.PlayerAnchor.Player;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public struct OnEnemyKilledByDamageEvent
    {
        public EnemyID Id { get; private set; }
        public Vector3 Position { get; private set; }
        public DamageHitResult DamageHitResult { get; private set; }
        public Dictionary<PlayerMovesetActions, int> TrackedPlayerActions { get; private set; }

        public OnEnemyKilledByDamageEvent(EnemyID id, Vector3 position, DamageHitResult damageHitResult,
            Dictionary<PlayerMovesetActions, int> trackedPlayerActions)
        {
            Id = id;
            Position = position;
            DamageHitResult = damageHitResult;
            TrackedPlayerActions = trackedPlayerActions;
        }
    }
    
    public class EnemyKilledByDamageEventData
    {
        public const string NAME = "Enemy Killed By Damage";
        public GenericEventData GenericEventData { get; private set; }
        public string EnemyName { get; private set; }
        public Vector3 Position { get; private set; }
        public string DamageHitName { get; private set; }
        
        public Dictionary<PlayerMovesetActions, int> TrackedPlayerActions { get; private set; }


        public EnemyKilledByDamageEventData(GenericEventData genericEventData, OnEnemyKilledByDamageEvent eventInfo)
        {
            GenericEventData = genericEventData;
            EnemyName = eventInfo.Id.GetEnemyName();
            Position = eventInfo.Position;

            DamageHitName = eventInfo.DamageHitResult.DamageHit.GetName();

            TrackedPlayerActions = eventInfo.TrackedPlayerActions;
        }
    }
}