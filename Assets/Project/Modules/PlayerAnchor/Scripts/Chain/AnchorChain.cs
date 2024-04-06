using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.InverseKinematics.Bones;
using Popeye.InverseKinematics.FABRIK;
using Popeye.Modules.PlayerAnchor.Player;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class AnchorChain : MonoBehaviour
    {
        [SerializeField] private BoneChain _boneChain;
        [SerializeField] private BoneChain _boneChainIK;
        [SerializeField] private FABRIKControllerBehaviour _controllerIK;
        private Transform _playerBindTransform;
        private Transform _anchorBindTransform;
        
        private IChainPhysics _chainPhysics;
        private IVFXChainView _vfxChainView;

        private IChainView _chainView;
        
        private IChainViewLogic _currentChainViewLogic;
        private SpiralThrowChainViewLogic _thrownChainViewLogic;
        private SpiralThrowChainViewLogic _pullChainViewLogic;
        private BoneChainChainViewLogic _restingOnFloorChainViewLogic;
        private FoldingChainViewLogic _dashingTowardsChainViewLogic;
        private SpiralThrowChainViewLogic _dashingAwayChainViewLogic;
        private FoldingChainViewLogic _carriedChainViewLogic;


        private Vector3 PlayerBindPosition => _playerBindTransform.position;
        private Vector3 AnchorBindPosition => _anchorBindTransform.position;
        
        
        public void Configure(IChainPhysics chainPhysics, IVFXChainView vfxChainView,
            Transform playerBindTransform, Transform anchorBindTransform,
            ChainViewLogicGeneralConfig generalConfig)
        {
            _chainPhysics = chainPhysics;
            _playerBindTransform = playerBindTransform;
            _anchorBindTransform = anchorBindTransform;
            
            float boneLength = generalConfig.MaxChainLength / (generalConfig.ChainBoneCount-1);

            _vfxChainView = vfxChainView;
            
            _chainView = new BoneChainChainView(_boneChain, generalConfig.ChainBoneCount,
                generalConfig.MaxChainLength, boneLength,
                generalConfig.BonePrefab, generalConfig.BoneEndEffectorPrefab);
            
            _thrownChainViewLogic = 
                new SpiralThrowChainViewLogic(generalConfig.ThrowViewLogicConfig, 
                    generalConfig.ChainBoneCount);
            
            _pullChainViewLogic = 
                new SpiralThrowChainViewLogic(generalConfig.PullViewLogicConfig, 
                    generalConfig.ChainBoneCount);
            
            _restingOnFloorChainViewLogic = 
                new BoneChainChainViewLogic(generalConfig.RestingOnFloorViewLogicConfig, 
                    generalConfig.ChainBoneCount, _boneChainIK, _controllerIK);

            _dashingTowardsChainViewLogic =
                new FoldingChainViewLogic(generalConfig.DashingTowardsViewLogicConfig,
                    generalConfig.ChainBoneCount);

            _carriedChainViewLogic = 
                new FoldingChainViewLogic(generalConfig.PickedUpTowardsViewLogicConfig,
                    generalConfig.ChainBoneCount);

            _dashingAwayChainViewLogic = 
                new SpiralThrowChainViewLogic(generalConfig.DashingAwayViewLogicConfig, 
                    generalConfig.ChainBoneCount);
            
            _currentChainViewLogic = _carriedChainViewLogic;
            SetCarriedView();
            
            _boneChainIK.AwakeConfigure(generalConfig.ChainBoneCount, false, boneLength);
        }


        private void LateUpdate()
        {
            _currentChainViewLogic.UpdateChainPositions(Time.deltaTime, PlayerBindPosition, AnchorBindPosition);

            Vector3[] newChainPositions = GetChainPositions();
            _chainView.Update(newChainPositions);
            
            newChainPositions = _chainView.GetUpdatedPositions();
            _vfxChainView.Update(newChainPositions);
        }

        public Vector3[] GetChainPositions()
        {
            return _currentChainViewLogic.GetChainPositions();
        }

        public void SetThrownView(AnchorThrowResult throwResult)
        {
            _thrownChainViewLogic.EnterSetup(throwResult.Duration);
            TransitionViewLogic(_thrownChainViewLogic);
            
            PlayChainThrowAnimation(throwResult);
        }
        public void SetPulledView(AnchorThrowResult pullResult)
        {
            _pullChainViewLogic.EnterSetup(pullResult.Duration);
            TransitionViewLogic(_pullChainViewLogic);
            
            PlayChainPullAnimation(pullResult);
        }
        public void SetRestingOnFloorView()
        {
            TransitionViewLogic(_restingOnFloorChainViewLogic);
        }
        public void SetCarriedView()
        {
            _carriedChainViewLogic.EnterSetup(0.15f, Ease.InOutSine);
            TransitionViewLogic(_carriedChainViewLogic);
        }
        public void SetDashingTowardsView(float dashDuration, Ease dashEase)
        {
            _dashingTowardsChainViewLogic.EnterSetup(dashDuration, dashEase);
            TransitionViewLogic(_dashingTowardsChainViewLogic);
        }
        public void SetDashingAwayView(float dashDuration, Ease dashEase)
        {
            _dashingAwayChainViewLogic.EnterSetup(dashDuration);
            TransitionViewLogic(_dashingAwayChainViewLogic);
        }

        private void TransitionViewLogic(IChainViewLogic newViewLogic)
        {
            Vector3[] previousStateChainPositions = _currentChainViewLogic.GetChainPositions();
            _currentChainViewLogic.OnViewExit();
            _currentChainViewLogic = newViewLogic;
            _currentChainViewLogic.OnViewEnter(previousStateChainPositions, PlayerBindPosition, AnchorBindPosition);
        }
        

        public void EnableTension()
        {
            _chainPhysics.EnableTension();
        }
        
        public void DisableTension()
        {
            _chainPhysics.DisableTension();
        }

        public async UniTaskVoid DisableTensionForDuration(float duration)
        {
            DisableTension();
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            EnableTension();
        }

        public void SetFailedThrow(bool failedThrow)
        {
            _chainPhysics.SetFailedThrow(failedThrow);
        }


        private void PlayChainThrowAnimation(AnchorThrowResult throwResult)
        {
            _vfxChainView.StartOriginAnimation(
                throwResult.FirstTrajectoryPathPoint + 
                Vector3.down * 0.5f + 
                throwResult.Direction * 2.0f,
                throwResult.Duration * 1.2f);
        }
        private void PlayChainPullAnimation(AnchorThrowResult pullResult)
        {
            _vfxChainView.StartOriginAnimation(
                pullResult.FirstTrajectoryPathPoint + 
                Vector3.down * 1.0f + 
                pullResult.Direction * 0.0f,
                pullResult.Duration * 0.75f);
        }
    }
}