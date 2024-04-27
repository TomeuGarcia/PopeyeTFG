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
        [SerializeField] private Material _normalBoneMaterial;
        [SerializeField] private Material _specialBoneMaterial;
        [SerializeField] private MeshRenderer[] _circleChainMeshes;
        [SerializeField] private float _disappearDuration = 0.1f;
        [SerializeField] private float _punchAmount = 2.0f;
        [SerializeField] private float _punchDuration = 1.0f;

        [SerializeField] private MovePunchBehaviour[] _movePunchBehaviours;
        [SerializeField] private AbilityUnlockerChristalViewConfig _viewConfig;
        
        [SerializeField] private GameObject _pointLight;

        public void Init()
        {
        }

        public void ResetState()
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            
            foreach (MeshRenderer meshRenderer in _circleChainMeshes)
            {
                meshRenderer.material = _normalBoneMaterial;
                meshRenderer.gameObject.SetActive(true);
            }
            
            foreach (MovePunchBehaviour movePunchBehaviour in _movePunchBehaviours)
            {
                movePunchBehaviour.Resume();
            }
            
            _pointLight.SetActive(true);
        }


        public async UniTask PlayDisappearAnimation(Vector3 hitOrigin)
        {
            foreach (MovePunchBehaviour movePunchBehaviour in _movePunchBehaviours)
            {
                movePunchBehaviour.Stop();
            }

            Vector3 direction = Vector3.ProjectOnPlane(transform.position - hitOrigin, Vector3.up).normalized;
            transform.DOPunchPosition(
                direction * _punchAmount,
                _punchDuration,
                vibrato: 1
            );
            await UniTask.Delay(TimeSpan.FromSeconds(_punchDuration / 2));
        
            foreach (MeshRenderer meshRenderer in _circleChainMeshes)
            {
                meshRenderer.material = _specialBoneMaterial;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(_disappearDuration));
            foreach (MeshRenderer meshRenderer in _circleChainMeshes)
            {
                meshRenderer.gameObject.SetActive(false);
            }
        }
        
        public async UniTask MoveToTarget(Transform target)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_viewConfig.CoreMoveDelay));
            
            Vector3 startPosition = transform.position;
            ScaleDown().Forget();
            
            Timer moveToTargetTimer = new Timer(_viewConfig.CoreMoveDuration);
            while (!moveToTargetTimer.HasFinished())
            {
                float t = _viewConfig.CoreMoveEase.Evaluate(moveToTargetTimer.GetCounterRatio01());
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
            
            _pointLight.SetActive(false);
        }

        private async UniTaskVoid ScaleDown()
        {
            await transform.Scale(_viewConfig.CoreMoveScale).AsyncWaitForCompletion();
            transform.Scale(_viewConfig.CoreFinalScale);
        }
        
    }
}