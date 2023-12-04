using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.PlayerAnchor.Player;
using Project.Modules.CombatSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class AnchorDamageDealer : MonoBehaviour
    {
        private AnchorDamageConfig _config;
        private Transform _damageStartTransform;
        private TransformMotion _damageTriggerMotion;
        [SerializeField] private DamageTrigger _anchorThrowDamageTrigger;
        

        public void Configure(AnchorDamageConfig anchorDamageConfig, ICombatManager combatManager, 
            Transform damageStartTransform)
        {
            _config = anchorDamageConfig;
            _damageStartTransform = damageStartTransform;
            _anchorThrowDamageTrigger.Configure(combatManager, new DamageHit(_config.AnchorThrowDamageHit));
            _anchorThrowDamageTrigger.Deactivate();

            _damageTriggerMotion = new TransformMotion();
            _damageTriggerMotion.Configure(_anchorThrowDamageTrigger.transform);
        }


        public async UniTaskVoid DealThrowDamage(AnchorThrowResult anchorThrowResult)
        {
            Vector3[] damagePathPoints = new Vector3[anchorThrowResult.TrajectoryPathPoints.Length];
            anchorThrowResult.TrajectoryPathPoints.CopyTo(damagePathPoints, 0);
            damagePathPoints[0] = _damageStartTransform.position;
            
            _damageTriggerMotion.SetPosition(damagePathPoints[0]);
            _damageTriggerMotion.SetRotation(_damageStartTransform.rotation);
            _damageTriggerMotion.MoveAlongPath(damagePathPoints, anchorThrowResult.Duration);

            _anchorThrowDamageTrigger.Activate();
            await UniTask.Delay(TimeSpan.FromSeconds(anchorThrowResult.Duration));
            _anchorThrowDamageTrigger.Deactivate();
        }
        
        
        
    }
}