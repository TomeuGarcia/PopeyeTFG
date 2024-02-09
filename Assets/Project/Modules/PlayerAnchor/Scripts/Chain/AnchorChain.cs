using System;
using DG.Tweening;
using Popeye.InverseKinematics.Bones;
using Popeye.InverseKinematics.FABRIK;
using Popeye.Modules.PlayerAnchor.Player;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class AnchorChain : MonoBehaviour
    {
        [SerializeField] private LineRenderer _chainLine;
        [SerializeField] private BoneChain _boneChain;
        [SerializeField] private BoneChain _boneChainIK;
        [SerializeField] private FABRIKControllerBehaviour _controllerIK;
        [SerializeField] private Material _chainSharedMaterial;
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
        private IChainViewLogic _carriedChainViewLogic;


        private Vector3 PlayerBindPosition => _playerBindTransform.position;
        private Vector3 AnchorBindPosition => _anchorBindTransform.position;
        
        
        public void Configure(IChainPhysics chainPhysics, Transform playerBindTransform, Transform anchorBindTransform,
            ChainViewLogicGeneralConfig chainViewLogicGeneralConfig)
        {
            _chainPhysics = chainPhysics;
            _playerBindTransform = playerBindTransform;
            _anchorBindTransform = anchorBindTransform;

            float boneLength = chainViewLogicGeneralConfig.MaxChainLength / (chainViewLogicGeneralConfig.ChainBoneCount-1);

            _vfxChainView = new VFXChainView(chainViewLogicGeneralConfig.ObstacleCollisionProbingConfig, _chainSharedMaterial);
            
            _chainView = new BoneChainChainView(_boneChain, chainViewLogicGeneralConfig.ChainBoneCount,
                chainViewLogicGeneralConfig.MaxChainLength, boneLength);
            //_chainView = new LineRendererChainView(_chainLine);
            
            
            _thrownChainViewLogic = 
                new SpiralThrowChainViewLogic(chainViewLogicGeneralConfig.ThrowViewLogicConfig, 
                    chainViewLogicGeneralConfig.ChainBoneCount);
            
            _pullChainViewLogic = 
                new SpiralThrowChainViewLogic(chainViewLogicGeneralConfig.PullViewLogicConfig, 
                    chainViewLogicGeneralConfig.ChainBoneCount);
            
            _restingOnFloorChainViewLogic = 
                new BoneChainChainViewLogic(chainViewLogicGeneralConfig.RestingOnFloorViewLogicConfig, 
                    chainViewLogicGeneralConfig.ChainBoneCount, _boneChainIK, _controllerIK);
            
            _carriedChainViewLogic = 
                new StraightLineChainViewLogic(chainViewLogicGeneralConfig.ChainBoneCount);

            _dashingTowardsChainViewLogic =
                new FoldingChainViewLogic(chainViewLogicGeneralConfig.DashingTowardsViewLogicConfig,
                    chainViewLogicGeneralConfig.ChainBoneCount);

            _dashingAwayChainViewLogic = 
                new SpiralThrowChainViewLogic(chainViewLogicGeneralConfig.DashingAwayViewLogicConfig, 
                    chainViewLogicGeneralConfig.ChainBoneCount);
            
            _currentChainViewLogic = _carriedChainViewLogic;
            
            _boneChainIK.AwakeConfigure(chainViewLogicGeneralConfig.ChainBoneCount, false, boneLength);
        }


        private void LateUpdate()
        {
            _currentChainViewLogic.UpdateChainPositions(Time.deltaTime, PlayerBindPosition, AnchorBindPosition);

            Vector3[] newChainPositions = _currentChainViewLogic.GetChainPositions();
            _chainView.Update(newChainPositions);
            _vfxChainView.Update(newChainPositions);
        }

        public void SetThrownView(AnchorThrowResult throwResult)
        {
            Vector3[] previousStateChainPositions = _currentChainViewLogic.GetChainPositions();
            _currentChainViewLogic.OnViewExit();
            _thrownChainViewLogic.EnterSetup(throwResult.Duration);
            _currentChainViewLogic = _thrownChainViewLogic;
            _currentChainViewLogic.OnViewEnter(previousStateChainPositions, PlayerBindPosition, AnchorBindPosition);
        }
        public void SetPulledView(AnchorThrowResult pullResult)
        {
            Vector3[] previousStateChainPositions = _currentChainViewLogic.GetChainPositions();
            _pullChainViewLogic.EnterSetup(pullResult.Duration);
            _currentChainViewLogic.OnViewExit();
            _currentChainViewLogic = _pullChainViewLogic;
            _currentChainViewLogic.OnViewEnter(previousStateChainPositions, PlayerBindPosition, AnchorBindPosition);
        }
        public void SetRestingOnFloorView()
        {
            Vector3[] previousStateChainPositions = _currentChainViewLogic.GetChainPositions();
            _currentChainViewLogic.OnViewExit();
            _currentChainViewLogic = _restingOnFloorChainViewLogic;
            _currentChainViewLogic.OnViewEnter(previousStateChainPositions, PlayerBindPosition, AnchorBindPosition);
        }
        public void SetCarriedView()
        {
            Vector3[] previousStateChainPositions = _currentChainViewLogic.GetChainPositions();
            _chainLine.enabled = true;
            _currentChainViewLogic.OnViewExit();
            _currentChainViewLogic = _carriedChainViewLogic;
            _currentChainViewLogic.OnViewEnter(previousStateChainPositions, PlayerBindPosition, AnchorBindPosition);
        }
        public void SetDashingTowardsView(float dashDuration, Ease dashEase)
        {
            Vector3[] previousStateChainPositions = _currentChainViewLogic.GetChainPositions();
            _dashingTowardsChainViewLogic.EnterSetup(dashDuration, dashEase);
            _currentChainViewLogic.OnViewExit();
            _currentChainViewLogic = _dashingTowardsChainViewLogic;
            _currentChainViewLogic.OnViewEnter(previousStateChainPositions, PlayerBindPosition, AnchorBindPosition);
        }
        public void SetDashingAwayView(float dashDuration, Ease dashEase)
        {
            Vector3[] previousStateChainPositions = _currentChainViewLogic.GetChainPositions();
            _currentChainViewLogic.OnViewExit();
            _dashingAwayChainViewLogic.EnterSetup(dashDuration);
            _currentChainViewLogic = _dashingAwayChainViewLogic;
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

        public void SetFailedThrow(bool failedThrow)
        {
            _chainPhysics.SetFailedThrow(failedThrow);
        }
        
        
    }
}