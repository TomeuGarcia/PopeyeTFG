using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.InverseKinematics.Bones;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public class ChainedOrbChainView : MonoBehaviour
    {
        [Header("CHAIN TARGET")]
        [SerializeField] private Transform _chainTarget;
        
        [Header("TO END")]
        [SerializeField] private Transform _chainTargetEndSpot;
        [SerializeField] private TweenEaseConfig _toEndSpotEase;
        
        [Header("BONE CHAIN")]
        [SerializeField] private BoneChain _boneChain;
        
        [Header("VIEW RENDERERS")]
        [SerializeField] private MeshRenderer _boneChainTarget;
        [SerializeField] private MeshRenderer _plateMesh;
        [SerializeField] private TrailRenderer _trail;

        private ChainedOrbViewConfig _viewConfig;
        private ChainedOrbViewConfig.UpgradeTypeToViewData _typeViewData;
        private Vector3 _originalLocalPosition;
        
        
        public void Init(ChainedOrbViewConfig viewConfig, ChainedOrbViewConfig.UpgradeTypeToViewData typeViewData)
        {
            _viewConfig = viewConfig;
            _typeViewData = typeViewData;
            _originalLocalPosition = _chainTarget.localPosition;            
        }

        public void ResetState(Transform originalChainTargetHolder)
        {
            _chainTarget.parent = originalChainTargetHolder;
            _chainTarget.localPosition = _originalLocalPosition;

            
            _boneChainTarget.material = _viewConfig.NormalChainMaterial;
            _boneChainTarget.gameObject.SetActive(true);
            _boneChain.SetMaterialToBones(_viewConfig.NormalChainMaterial);
            _boneChain.Show();

            _plateMesh.material = _viewConfig.NormalChainMaterial;
            _plateMesh.gameObject.SetActive(true);
            
            _trail.emitting = true;
        }
        
        
        public async UniTaskVoid PlayDisappearAnimation(Transform finishChainTargetHolder)
        {
            _chainTarget.parent = finishChainTargetHolder;

            _chainTarget.DOMove(_chainTargetEndSpot.position, _toEndSpotEase.Duration)
                .SetEase(_toEndSpotEase.Ease);

            
            _trail.emitting = false;

            
            _boneChainTarget.material = _typeViewData.BreakingChainMaterial;
            await UniTask.Delay(TimeSpan.FromSeconds(_viewConfig.BreakStepDuration.x));
            _boneChainTarget.gameObject.SetActive(false);

            for (int i = _boneChain.NumberOfBones - 1; i >= 0; --i)
            {
                Bone bone = _boneChain.Bones[i];
                bone.SetMaterial(_typeViewData.BreakingChainMaterial);

                float t = 1 - ((float)i / _boneChain.NumberOfBones);
                float duration = Mathf.Lerp(_viewConfig.BreakStepDuration.x, _viewConfig.BreakStepDuration.y, t);
                
                bone.transform.DOPunchScale(_viewConfig.BreakPunch, duration);
                await UniTask.Delay(TimeSpan.FromSeconds(duration));
                bone.Hide();
            }

            _plateMesh.material = _typeViewData.BreakingChainMaterial;
            _plateMesh.transform.DOPunchScale(_viewConfig.BreakPunch, _viewConfig.BreakStepDuration.x);
            await UniTask.Delay(TimeSpan.FromSeconds(_viewConfig.BreakStepDuration.x));
            _plateMesh.gameObject.SetActive(false);
        }
        
        
    }
}