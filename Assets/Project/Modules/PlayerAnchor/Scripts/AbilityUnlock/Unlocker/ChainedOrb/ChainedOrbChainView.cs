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
        private Vector3 _originalLocalPosition;
        [SerializeField] private Transform _chainTarget;
        [SerializeField] private Transform _chainTargetEndSpot;
        [SerializeField] private TweenEaseConfig _toEndSpotEase;
        [SerializeField] private BoneChain _boneChain;
        [SerializeField] private MeshRenderer _boneChainTarget;
        [SerializeField] private Material _normalBoneMaterial;
        [SerializeField] private Material _specialBoneMaterial;
        [SerializeField] private TrailRenderer _trail;
        [SerializeField] private Vector2 _disappearStepDuration = new Vector2(0.1f, 0.2f);
        [SerializeField] private Vector3 _disappearPunch = Vector3.one * 0.5f;
        [SerializeField] private MeshRenderer _plateMesh;

        public void Init()
        {
            _originalLocalPosition = _chainTarget.localPosition;            
        }

        public void ResetState(Transform originalChainTargetHolder)
        {
            _chainTarget.parent = originalChainTargetHolder;
            _chainTarget.localPosition = _originalLocalPosition;

            
            _boneChainTarget.material = _normalBoneMaterial;
            _boneChainTarget.gameObject.SetActive(true);
            _boneChain.SetMaterialToBones(_normalBoneMaterial);
            _boneChain.Show();

            _plateMesh.material = _normalBoneMaterial;
            _plateMesh.gameObject.SetActive(true);
            
            _trail.emitting = true;
        }
        
        
        public async UniTaskVoid PlayDisappearAnimation(Transform finishChainTargetHolder)
        {
            _chainTarget.parent = finishChainTargetHolder;

            _chainTarget.DOMove(_chainTargetEndSpot.position, _toEndSpotEase.Duration)
                .SetEase(_toEndSpotEase.Ease);

            
            _trail.emitting = false;

            
            _boneChainTarget.material = _specialBoneMaterial;
            await UniTask.Delay(TimeSpan.FromSeconds(_disappearStepDuration.x));
            _boneChainTarget.gameObject.SetActive(false);

            for (int i = _boneChain.NumberOfBones - 1; i >= 0; --i)
            {
                Bone bone = _boneChain.Bones[i];
                bone.SetMaterial(_specialBoneMaterial);

                float duration = Mathf.Lerp(_disappearStepDuration.x, _disappearStepDuration.y,
                    1 - ((float)i / _boneChain.NumberOfBones));
                bone.transform.DOPunchScale(_disappearPunch, duration);
                await UniTask.Delay(TimeSpan.FromSeconds(duration));
                bone.Hide();
            }

            _plateMesh.material = _specialBoneMaterial;
            _plateMesh.transform.DOPunchScale(_disappearPunch, _disappearStepDuration.x);
            await UniTask.Delay(TimeSpan.FromSeconds(_disappearStepDuration.x));
            _plateMesh.gameObject.SetActive(false);
        }
        
        
    }
}