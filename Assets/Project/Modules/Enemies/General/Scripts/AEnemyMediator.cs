using System;
using Popeye.Core.Pool;
using Popeye.Core.Services.EventSystem;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies.Components;
using Popeye.Modules.Enemies.General;
using Popeye.Modules.Enemies.Hazards;
using Popeye.Modules.Enemies.VFX;
using Popeye.Modules.GameDataEvents;
using UnityEngine;

namespace Popeye.Modules.Enemies
{
    public abstract class AEnemyMediator : RecyclableObject
    {
        [SerializeField] protected EnemyHealth _enemyHealth;
        [SerializeField] protected EnemyVisuals _enemyVisuals;
        protected IHazardFactory _hazardsFactory;
        protected IEventSystemService _eventSystem;
        [SerializeField] private EnemyID _enemyID;
        public abstract Vector3 Position { get; }

        public struct EnemyStartsFightingPlayer { }
        public struct EnemyStopsFightingPlayer { }

        private void OnEnable()
        {
            _fightsStarted = 0;
            _fightsStopped = 0;
        }

        public virtual void OnHit(DamageHitResult damageHitResult)
        {
            _enemyVisuals.PlayHitEffects(_enemyHealth.GetValuePer1Ratio(), damageHitResult.DamageHit);
            _eventSystem.Dispatch(new OnEnemyTakeDamageEvent(_enemyID,transform.position,damageHitResult));
        }

        public virtual void OnSeePlayer()
        {
            _eventSystem.Dispatch(new OnEnemySeesPlayerEvent(_enemyID));
            InvokeEnemyStartsFightingPlayer();
        }

        public virtual void OnDeath(DamageHitResult damageHitResult)
        {
            _enemyVisuals.PlayDeathEffects(damageHitResult.DamageHit);
            InvokeEnemyStopsFightingPlayer();
            Recycle();
        }

        public void SetHazardFactory(IHazardFactory hazardsFactory)
        {
            _hazardsFactory = hazardsFactory;
        }

        public void SetEventSystem(IEventSystemService eventSystem)
        {
            _eventSystem = eventSystem;
        }

        public virtual void OnPlayerClose()
        {
            OnSeePlayer();
        }
        public virtual void OnPlayerFar()
        {
            InvokeEnemyStopsFightingPlayer();
        }

        public abstract void DieFromOrder();


        private int _fightsStarted = 0;
        private int _fightsStopped = 0;
        protected void InvokeEnemyStartsFightingPlayer()
        {
            ++_fightsStarted;
            if (_fightsStarted - _fightsStopped != 1)
            {
                --_fightsStarted;
                // THS COMMENT IS HERE TO STAY TO SHAME THIS CODE
                // Debug.Log("AQUI L'HAURIA PUTO CAGAT - 1 : " + name);
                return;
            }
            _eventSystem.Dispatch(new EnemyStartsFightingPlayer());
        }
        protected void InvokeEnemyStopsFightingPlayer()
        {
            ++_fightsStopped;
            if (_fightsStarted - _fightsStopped != 0)
            {
                --_fightsStopped;
                // THS COMMENT IS HERE TO STAY TO SHAME THIS CODE
                // Debug.Log("AQUI L'HAURIA PUTO CAGAT - 2 : " + name);
                return;
            }
            _eventSystem.Dispatch(new EnemyStopsFightingPlayer());
        }
    }
}