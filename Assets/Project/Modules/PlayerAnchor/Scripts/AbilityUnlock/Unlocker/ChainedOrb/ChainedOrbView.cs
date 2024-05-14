using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.Utilities.Scripts;
using Popeye.Timers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public class ChainedOrbView : MonoBehaviour
    {
        [Header("ORB")]
        [SerializeField] private MeshRenderer _orbMesh;
        [SerializeField] private MeshRenderer _outterOrb;
        
        [Header("CHAINS")]
        [SerializeField] private Light _pointLight;
        [SerializeField] private MeshRenderer[] _circleChainMeshes;
        
        [Header("MOVEMENT")]
        [SerializeField] private MovePunchBehaviour[] _movePunchBehaviours;
        
        private ChainedOrbViewConfig _viewConfig;
        private ChainedOrbViewConfig.UpgradeTypeToViewData _typeViewData;
        

        public void Init(ChainedOrbViewConfig viewConfig, ChainedOrbViewConfig.UpgradeTypeToViewData typeViewData)
        {
            _viewConfig = viewConfig;
            _typeViewData = typeViewData;

            _orbMesh.material = _typeViewData.OrbMaterial;
            _outterOrb.material = _typeViewData.OutterOrbMaterial;
            _pointLight.color = _typeViewData.LightColor;
        }

        public void ResetState()
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            
            foreach (MeshRenderer meshRenderer in _circleChainMeshes)
            {
                meshRenderer.material = _viewConfig.NormalChainMaterial;
                meshRenderer.gameObject.SetActive(true);
            }
            
            foreach (MovePunchBehaviour movePunchBehaviour in _movePunchBehaviours)
            {
                movePunchBehaviour.Resume();
            }
            
            _pointLight.gameObject.SetActive(true);
        }


        public async UniTask PlayDisappearAnimation(Vector3 hitOrigin)
        {
            foreach (MovePunchBehaviour movePunchBehaviour in _movePunchBehaviours)
            {
                movePunchBehaviour.Stop();
            }

            Vector3 direction = Vector3.ProjectOnPlane(transform.position - hitOrigin, Vector3.up).normalized;
            transform.DOLocalMove(
                direction * _viewConfig.OrbHitMovePunch,
                _viewConfig.OrbHitDuration
            ).SetEase(Ease.OutCirc);
            transform.DOBlendableLocalRotateBy(
                Vector3.up * _viewConfig.OrbHitRotateAmount, 
                _viewConfig.OrbHitDuration)
                .SetEase(Ease.OutCirc);
                
            await UniTask.Delay(TimeSpan.FromSeconds(_viewConfig.OrbChainsStartBreakingDelay));
        
            foreach (MeshRenderer meshRenderer in _circleChainMeshes)
            {
                meshRenderer.material = _typeViewData.BreakingChainMaterial;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(_viewConfig.OrbChainsDisappearDelay));
            
            foreach (MeshRenderer meshRenderer in _circleChainMeshes)
            {
                meshRenderer.gameObject.SetActive(false);
            }
        }
        
        public async UniTask MoveToTarget(Transform target)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_viewConfig.OrbMoveToTarget.StartDelay));
            
            Vector3 startPosition = transform.position;
            ScaleDown().Forget();
            
            Timer moveToTargetTimer = new Timer(_viewConfig.OrbMoveToTarget.MoveDuration);
            while (!moveToTargetTimer.HasFinished())
            {
                float t = _viewConfig.OrbMoveToTarget.MoveEase.Evaluate(moveToTargetTimer.GetCounterRatio01());
                Vector3 endPosition = target.position;

                Vector3 currentPosition = Vector3.LerpUnclamped(startPosition, endPosition, t);
                transform.position = currentPosition;

                Vector3 currentToEnd = endPosition - currentPosition;
                float currentToEndDistance = currentToEnd.magnitude;
                if (currentToEndDistance > 0.001f)
                {
                    transform.forward = currentToEnd / currentToEndDistance;
                }

                moveToTargetTimer.Update(Time.deltaTime);
                await UniTask.Yield();
            }
            
            _pointLight.gameObject.SetActive(false);
        }

        private async UniTaskVoid ScaleDown()
        {
            await transform.Scale(_viewConfig.OrbMoveToTarget.StartScale).AsyncWaitForCompletion();
            transform.Scale(_viewConfig.OrbMoveToTarget.EndScale);
        }
        
    }
}