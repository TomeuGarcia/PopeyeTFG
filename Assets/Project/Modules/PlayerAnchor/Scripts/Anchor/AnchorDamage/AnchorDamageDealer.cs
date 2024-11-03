using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor;
using Popeye.Modules.PlayerAnchor.Player;
using Popeye.Modules.CombatSystem;
using Popeye.Scripts.TransformUtilities;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class AnchorDamageDealer : MonoBehaviour
    {
        private IAnchorDamageDealerListener _listener;
        private AnchorDamageConfig _config;
        private Transform _damageStartTransform;
        
        private RigidbodyMotion _throwDamageTriggerMotion;
        private RigidbodyMotion _spinDamageTriggerMotion;
        
        [Header("THROW")]
        [SerializeField] private DamageTrigger _anchorThrowDamageTrigger;
        [SerializeField] private Rigidbody _anchorThrowRigidbody;
        [Header("SLAM")]
        [SerializeField] private DamageTrigger _anchorVerticalLandDamageTrigger;
        [Header("SPIN")]
        [SerializeField] private DamageTrigger _anchorSpinDamageTrigger;
        [SerializeField] private BoxCollider _anchorSpinCollider;
        [SerializeField] private Rigidbody _anchorSpinRigidbody;

        private bool _sidewaysKnockbackIsRight;

        private DamageHit ThrowDamageHit => _config.ThrowDamageHit;
        private DamageHit PullDamageHit => _config.PullDamageHit;
        private DamageHit SpinDamageHit => _config.SpinDamageHit;
        private DamageHit VerticalLandDamageHit => _config.VerticalLandDamageHit;

        public void Configure(IAnchorDamageDealerListener listener,
            AnchorDamageConfig anchorDamageConfig, ICombatManager combatManager, 
            Transform damageStartTransform)
        {
            _listener = listener;
            
            _config = anchorDamageConfig;
            _config.Init();
            
            _damageStartTransform = damageStartTransform;


            _anchorThrowDamageTrigger.Configure(new DamageDealer(combatManager));
            _anchorThrowDamageTrigger.Deactivate();

            _anchorSpinDamageTrigger.Configure(new DamageDealer(combatManager), SpinDamageHit);
            _anchorSpinDamageTrigger.Deactivate();
            
            
            _anchorVerticalLandDamageTrigger.Configure(new DamageDealer(combatManager));
            _anchorVerticalLandDamageTrigger.Deactivate();
            

            _throwDamageTriggerMotion = new RigidbodyMotion();
            _throwDamageTriggerMotion.Configure(_anchorThrowRigidbody);

            _spinDamageTriggerMotion = new RigidbodyMotion();
            _spinDamageTriggerMotion.Configure(_anchorSpinRigidbody);

            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _anchorThrowDamageTrigger.OnDamageDealt += OnDamageDealt;
            _anchorSpinDamageTrigger.OnDamageDealt += OnDamageDealt;
            _anchorVerticalLandDamageTrigger.OnDamageDealt += OnDamageDealt;
            _anchorVerticalLandDamageTrigger.OnBeforeDamageDealt += SetPushAwayFromOriginKnockback;
        }
        private void UnsubscribeToEvents()
        {
            _anchorThrowDamageTrigger.OnDamageDealt -= OnDamageDealt;
            _anchorSpinDamageTrigger.OnDamageDealt -= OnDamageDealt;
            _anchorVerticalLandDamageTrigger.OnDamageDealt -= OnDamageDealt;
            _anchorVerticalLandDamageTrigger.OnBeforeDamageDealt -= SetPushAwayFromOriginKnockback;
        }
        


        public void DealThrowDamage(AnchorThrowResult anchorThrowResult)
        {
            _anchorThrowDamageTrigger.SetDamageHit(ThrowDamageHit);
            _anchorThrowDamageTrigger.UpdateDamageKnockbackDirection(anchorThrowResult.Direction);
            
            
            Vector3[] damagePathPoints = new Vector3[anchorThrowResult.TrajectoryPathPoints.Length];
            anchorThrowResult.TrajectoryPathPoints.CopyTo(damagePathPoints, 0);
            damagePathPoints[0] = _damageStartTransform.position;
            
            DealTrajectoryDamage(anchorThrowResult.TrajectoryPathPoints, 
                    anchorThrowResult.Duration, _config.ThrowDamageExtraDuration,
                    anchorThrowResult.MoveEaseCurve, -1.0f)
                .Forget();
        }

        public async UniTaskVoid DealPullDamage(AnchorThrowResult anchorPullResult)
        {
            _anchorThrowDamageTrigger.OnBeforeDamageDealt += SetPullAttackKnockbackEndPosition;
            await DoDealPullDamage(anchorPullResult);
            _anchorThrowDamageTrigger.OnBeforeDamageDealt -= SetPullAttackKnockbackEndPosition;
        }
        
        private async UniTask DoDealPullDamage(AnchorThrowResult anchorThrowResult)
        {
            _anchorThrowDamageTrigger.SetDamageHit(PullDamageHit);
            _anchorThrowDamageTrigger.UpdateKnockbackEndPosition(anchorThrowResult.Direction);

            await DealTrajectoryDamage(anchorThrowResult.TrajectoryPathPoints,
                anchorThrowResult.Duration, _config.PullDamageExtraDuration,
                anchorThrowResult.MoveEaseCurve, -1.0f);
        }
        
        
        
        private async UniTask DealTrajectoryDamage(Vector3[] trajectoryPoints, float duration, float extraDurationBeforeDeactivate,
            AnimationCurve ease, float easeThreshold)
        {
            _throwDamageTriggerMotion.SetPositionIgnoringPhysics(trajectoryPoints[0]);
            _throwDamageTriggerMotion.SetRotationIgnoringPhysics(_damageStartTransform.rotation);
            await UniTask.Yield();
            await UniTask.Yield();
            _throwDamageTriggerMotion.MoveAlongPath(trajectoryPoints, duration, ease);

            float wait = 0f;
            if (easeThreshold > 0)
            {
                wait = await WaitUntilEase(ease, duration, easeThreshold);    
            }
            
            _anchorThrowDamageTrigger.Activate();
            await UniTask.Delay(TimeSpan.FromSeconds(Mathf.Max(duration * (1f-wait), 0.1f)));
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
            _anchorVerticalLandDamageTrigger.SetDamageHit(VerticalLandDamageHit);
            _anchorVerticalLandDamageTrigger.UpdateDamageKnockbackDirection(anchorThrowResult.Direction);
            
            DealLandHitDamage(anchorThrowResult.TrajectoryPathPoints, 
                    anchorThrowResult.Duration, _config.VerticalLandDamageDuration)
                .Forget();
        }
        
        private async UniTaskVoid DealLandHitDamage(Vector3[] trajectoryPoints, float waitDuration, float damageDuration)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(waitDuration));
            
            _anchorVerticalLandDamageTrigger.transform.position = trajectoryPoints[^1];
            _anchorVerticalLandDamageTrigger.Activate();
            
            await UniTask.Delay(TimeSpan.FromSeconds(damageDuration));
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
        
        private void SetPullAttackKnockbackEndPosition(DamageTrigger damageTrigger, GameObject tryHitObject)
        {
            Vector3 originPosition = _damageStartTransform.position;
            Vector3 originToEndDirection = (tryHitObject.transform.position - originPosition).normalized;
            Vector3 endPosition = originPosition + (originToEndDirection * _config.PullKnockbackDistanceFromPlayer);
            
            damageTrigger.UpdateKnockbackEndPosition(endPosition);
        }

        
        
        
        public void StartDealingSpinDamage(bool spinningToTheRight)
        {
            _sidewaysKnockbackIsRight = spinningToTheRight;
            _anchorSpinDamageTrigger.Activate();
            _anchorSpinDamageTrigger.OnBeforeDamageDealt += SetPushSidewaysKnockback;
            
            _spinDamageTriggerMotion.SetPositionIgnoringPhysics(_damageStartTransform.position);
            _spinDamageTriggerMotion.SetRotationIgnoringPhysics(Quaternion.identity);
        }
        public void UpdateSpinningDamage(Vector3 spinCenter, Quaternion rotation, float spinRadius)
        {
            _spinDamageTriggerMotion.SetPosition(spinCenter);
            _spinDamageTriggerMotion.SetRotation(rotation);

            spinRadius += 1.5f; // Add extra collider size
            _anchorSpinCollider.size = new Vector3(2, 2, spinRadius);
            _anchorSpinCollider.center = Vector3.forward * (spinRadius / 2);
        }
        public void StopDealingSpinDamage()
        {
            _anchorSpinDamageTrigger.Deactivate();
            _anchorSpinDamageTrigger.OnBeforeDamageDealt -= SetPushSidewaysKnockback;
        }



        private void OnDamageDealt(DamageHitResult damageHitResult)
        {
            _listener.OnDamageDealt(damageHitResult);
        }
        
    }
}