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

        private DamageHit _throwDamageHit;
        private DamageHit _pullDamageHit;
        private DamageHit _kickDamageHit;

        public void Configure(AnchorDamageConfig anchorDamageConfig, ICombatManager combatManager, 
            Transform damageStartTransform)
        {
            _config = anchorDamageConfig;
            _damageStartTransform = damageStartTransform;

            _throwDamageHit = new DamageHit(_config.AnchorThrowDamageHit); 
            _pullDamageHit = new DamageHit(_config.AnchorPullDamageHit); 
            _kickDamageHit = new DamageHit(_config.AnchorKickDamageHit); 
            
            _anchorThrowDamageTrigger.Configure(combatManager, _throwDamageHit);
            _anchorThrowDamageTrigger.Deactivate();

            _damageTriggerMotion = new TransformMotion();
            _damageTriggerMotion.Configure(_anchorThrowDamageTrigger.transform);
        }


        public void DealThrowDamage(AnchorThrowResult anchorThrowResult)
        {
            DealForwardThrowDamage(anchorThrowResult, _throwDamageHit, _config.ThrowDamageExtraDuration);
        }

        public void DealPullDamage(AnchorThrowResult anchorPullResult)
        {
            DealBackwardThrowDamage(anchorPullResult, _pullDamageHit, _config.PullDamageExtraDuration);
        }
        
        public void DealKickDamage(AnchorThrowResult anchorKickResult)
        {
            DealForwardThrowDamage(anchorKickResult, _kickDamageHit, _config.KickDamageExtraDuration);
        }

        
        private void DealForwardThrowDamage(AnchorThrowResult anchorThrowResult, DamageHit damageHit, 
            float extraDurationBeforeDeactivate)
        {
            _anchorThrowDamageTrigger.SetDamageHit(damageHit);
            _anchorThrowDamageTrigger.UpdateDamageKnockbackDirection(anchorThrowResult.Direction);
            
            
            Vector3[] damagePathPoints = new Vector3[anchorThrowResult.TrajectoryPathPoints.Length];
            anchorThrowResult.TrajectoryPathPoints.CopyTo(damagePathPoints, 0);
            damagePathPoints[0] = _damageStartTransform.position;
            
            DealTrajectoryDamage(anchorThrowResult.TrajectoryPathPoints, 
                    anchorThrowResult.Duration, extraDurationBeforeDeactivate,
                    anchorThrowResult.InterpolationEaseCurve, -1.0f)
                .Forget();
        }

        private void DealBackwardThrowDamage(AnchorThrowResult anchorThrowResult, DamageHit damageHit,
            float extraDurationBeforeDeactivate)
        {
            _anchorThrowDamageTrigger.SetDamageHit(damageHit);
            _anchorThrowDamageTrigger.UpdateDamageKnockbackDirection(anchorThrowResult.Direction);
            
            DealTrajectoryDamage(anchorThrowResult.TrajectoryPathPoints, 
                    anchorThrowResult.Duration, extraDurationBeforeDeactivate,
                    anchorThrowResult.InterpolationEaseCurve, 0.1f)
                .Forget();
        }
        
        
        
        private async UniTaskVoid DealTrajectoryDamage(Vector3[] trajectoryPoints, float duration, float extraDurationBeforeDeactivate,
            AnimationCurve ease, float easeThreshold)
        {
            _damageTriggerMotion.SetPosition(trajectoryPoints[0]);
            _damageTriggerMotion.SetRotation(_damageStartTransform.rotation);
            _damageTriggerMotion.MoveAlongPath(trajectoryPoints, duration, ease);
            
            var wait = await WaitUntilEase(ease, duration, easeThreshold);
            _anchorThrowDamageTrigger.Activate();
            await UniTask.Delay(TimeSpan.FromSeconds(duration * (1f-wait)));
            await UniTask.Delay(TimeSpan.FromSeconds(extraDurationBeforeDeactivate));
            _anchorThrowDamageTrigger.Deactivate();
        }


        private async UniTask<float> WaitUntilEase(AnimationCurve ease, float duration, float easeThreshold)
        {
            float t = 0f;
            float curveT = 0f;
            while (curveT < easeThreshold)
            {
                t += Time.deltaTime / duration;
                curveT = ease.Evaluate(t);
                await UniTask.Yield();
            }

            return t;
        }


        private void PullDamageHitTarget(IDamageHitTarget damageHitTarget)
        {
            
        }
        
    }
}