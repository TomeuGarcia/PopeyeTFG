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
        
        private TransformMotion _throwDamageTriggerMotion;
        private TransformMotion _spinDamageTriggerMotion;
        
        [SerializeField] private DamageTrigger _anchorThrowDamageTrigger;
        [SerializeField] private DamageTrigger _anchorSpinDamageTrigger;
        [SerializeField] private DamageTrigger _anchorVerticalLandDamageTrigger;

        private bool _sidewaysKnockbackIsRight;
        
        private DamageHit _throwDamageHit;
        private DamageHit _pullDamageHit;
        private DamageHit _kickDamageHit;
        private DamageHit _spinDamageHit;
        private DamageHit _verticalLandDamageHit;

        public void Configure(AnchorDamageConfig anchorDamageConfig, ICombatManager combatManager, 
            Transform damageStartTransform)
        {
            _config = anchorDamageConfig;
            _damageStartTransform = damageStartTransform;

            _throwDamageHit = new DamageHit(_config.AnchorThrowDamageHit); 
            _pullDamageHit = new DamageHit(_config.AnchorPullDamageHit); 
            _kickDamageHit = new DamageHit(_config.AnchorKickDamageHit); 
            _verticalLandDamageHit = new DamageHit(_config.AnchorVerticalLandDamageHit); 
            _spinDamageHit = new DamageHit(_config.AnchorSpinDamageHit); 
            
            _anchorThrowDamageTrigger.Configure(combatManager, _throwDamageHit);
            _anchorThrowDamageTrigger.Deactivate();
            
            _anchorSpinDamageTrigger.Configure(combatManager, _spinDamageHit);
            _anchorSpinDamageTrigger.Deactivate();
            
            _anchorVerticalLandDamageTrigger.Configure(combatManager, _verticalLandDamageHit);
            _anchorVerticalLandDamageTrigger.Deactivate();
            _anchorVerticalLandDamageTrigger.OnBeforeDamageDealt += SetPushAwayFromOriginKnockback;

            _throwDamageTriggerMotion = new TransformMotion();
            _throwDamageTriggerMotion.Configure(_anchorThrowDamageTrigger.transform);

            _spinDamageTriggerMotion = new TransformMotion();
            _spinDamageTriggerMotion.Configure(_anchorSpinDamageTrigger.transform);
        }

        private void OnDestroy()
        {
            _anchorVerticalLandDamageTrigger.OnBeforeDamageDealt -= SetPushAwayFromOriginKnockback;
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
                    anchorThrowResult.MoveEaseCurve, -1.0f)
                .Forget();
        }

        private void DealBackwardThrowDamage(AnchorThrowResult anchorThrowResult, DamageHit damageHit,
            float extraDurationBeforeDeactivate)
        {
            _anchorThrowDamageTrigger.SetDamageHit(damageHit);
            _anchorThrowDamageTrigger.UpdateDamageKnockbackDirection(anchorThrowResult.Direction);
            
            DealTrajectoryDamage(anchorThrowResult.TrajectoryPathPoints, 
                    anchorThrowResult.Duration, extraDurationBeforeDeactivate,
                    anchorThrowResult.MoveEaseCurve, 0.1f)
                .Forget();
        }
        
        
        
        private async UniTaskVoid DealTrajectoryDamage(Vector3[] trajectoryPoints, float duration, float extraDurationBeforeDeactivate,
            AnimationCurve ease, float easeThreshold)
        {
            _throwDamageTriggerMotion.SetPosition(trajectoryPoints[0]);
            _throwDamageTriggerMotion.SetRotation(_damageStartTransform.rotation);
            _throwDamageTriggerMotion.MoveAlongPath(trajectoryPoints, duration, ease);
            
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

        
        
        public void DealVerticalLandDamage(AnchorThrowResult anchorThrowResult)
        {
            _anchorVerticalLandDamageTrigger.SetDamageHit(_verticalLandDamageHit);
            _anchorVerticalLandDamageTrigger.UpdateDamageKnockbackDirection(anchorThrowResult.Direction);
            
            DealLandHitDamage(anchorThrowResult.TrajectoryPathPoints, 
                    anchorThrowResult.Duration, _config.VerticalLandDamageExtraDuration,
                    anchorThrowResult.MoveEaseCurve, 0.6f)
                .Forget();
        }
        
        private async UniTaskVoid DealLandHitDamage(Vector3[] trajectoryPoints, float duration, float extraDurationBeforeDeactivate,
            AnimationCurve ease, float easeThreshold)
        {
            var wait = await WaitUntilEase(ease, duration, easeThreshold);
            
            _anchorVerticalLandDamageTrigger.transform.position = trajectoryPoints[^1];
            
            _anchorVerticalLandDamageTrigger.Activate();
            await UniTask.Delay(TimeSpan.FromSeconds(duration * (1f-wait)));
            await UniTask.Delay(TimeSpan.FromSeconds(extraDurationBeforeDeactivate));
            _anchorVerticalLandDamageTrigger.Deactivate();
        }



        private void SetPushAwayFromOriginKnockback(DamageTrigger damageTrigger, GameObject tryHitObject)
        {
            Vector3 pushDirection = tryHitObject.transform.position - damageTrigger.Position;
            pushDirection = Vector3.ProjectOnPlane(pushDirection, Vector3.up).normalized;
            
            damageTrigger.UpdateDamageKnockbackDirection(pushDirection);
        }
        
        private void SetPushSidewaysKnockback(DamageTrigger damageTrigger, GameObject tryHitObject)
        {
            Vector3 pushDirection = _sidewaysKnockbackIsRight
                ? damageTrigger.transform.right
                : -damageTrigger.transform.right;

            pushDirection = Vector3.ProjectOnPlane(pushDirection, Vector3.up).normalized;
            
            damageTrigger.UpdateDamageKnockbackDirection(pushDirection);
        }

        
        public void StartDealingSpinDamage(bool spinningToTheRight)
        {
            _sidewaysKnockbackIsRight = spinningToTheRight;
            _anchorSpinDamageTrigger.Activate();
            _anchorSpinDamageTrigger.OnBeforeDamageDealt += SetPushSidewaysKnockback;
        }
        public void UpdateSpinningDamage(Vector3 position, Quaternion rotation)
        {
            _spinDamageTriggerMotion.SetPosition(position);
            _spinDamageTriggerMotion.SetRotation(rotation);
            
        }
        public void StopDealingSpinDamage()
        {
            _anchorSpinDamageTrigger.Deactivate();
            _anchorSpinDamageTrigger.OnBeforeDamageDealt -= SetPushSidewaysKnockback;
        }
        
        
    }
}