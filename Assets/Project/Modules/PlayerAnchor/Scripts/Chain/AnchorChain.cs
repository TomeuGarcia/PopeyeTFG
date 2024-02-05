using System;
using Popeye.InverseKinematics.Bones;
using Popeye.Modules.PlayerAnchor.Player;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class AnchorChain : MonoBehaviour
    {
        [SerializeField] private LineRenderer _chainLine;
        [SerializeField] private Transform _chainIK;
        [SerializeField] private BoneChain _boneChainIK;
        private Transform _playerBindTransform;
        private Transform _anchorBindTransform;
        
        private IChainPhysics _chainPhysics;

        private IChainView _chainView;
        
        private IChainViewLogic _currentChainViewLogic;
        private SpiralThrowChainViewLogic _thrownChainViewLogic;
        private SpiralThrowChainViewLogic _pullChainViewLogic;
        private BoneChainChainViewLogic _restingOnFloorChainViewLogic;
        private IChainViewLogic _carriedChainViewLogic;

        
        public void Configure(IChainPhysics chainPhysics, Transform playerBindTransform, Transform anchorBindTransform,
            ChainViewLogicGeneralConfig chainViewLogicGeneralConfig)
        {
            _chainPhysics = chainPhysics;
            _playerBindTransform = playerBindTransform;
            _anchorBindTransform = anchorBindTransform;

            _chainView = new LineRendererChainView(_chainLine);
            
            _thrownChainViewLogic = new SpiralThrowChainViewLogic(chainViewLogicGeneralConfig.ThrowViewLogicConfig, chainViewLogicGeneralConfig.ChainBoneCount);
            _pullChainViewLogic = new SpiralThrowChainViewLogic(chainViewLogicGeneralConfig.PullViewLogicConfig, chainViewLogicGeneralConfig.ChainBoneCount);
            _restingOnFloorChainViewLogic = new BoneChainChainViewLogic(chainViewLogicGeneralConfig.ChainBoneCount, _chainIK, _boneChainIK);
            _carriedChainViewLogic = new StraightLineChainViewLogic(chainViewLogicGeneralConfig.ChainBoneCount);
            _currentChainViewLogic = _carriedChainViewLogic;
            
            _boneChainIK.AwakeConfigure(chainViewLogicGeneralConfig.ChainBoneCount);
        }


        private void LateUpdate()
        {
            _currentChainViewLogic.UpdateChainPositions(Time.deltaTime, _playerBindTransform.position, _anchorBindTransform.position);
            _chainView.Update(_currentChainViewLogic.GetChainPositions());
        }

        public void SetThrownView(AnchorThrowResult throwResult)
        {
            _currentChainViewLogic.OnViewExit();
            _thrownChainViewLogic.EnterSetup(throwResult);
            _currentChainViewLogic = _thrownChainViewLogic;
            _currentChainViewLogic.OnViewEnter();
        }
        public void SetPulledView(AnchorThrowResult pullResult)
        {
            _pullChainViewLogic.EnterSetup(pullResult);
            _currentChainViewLogic.OnViewExit();
            _currentChainViewLogic = _pullChainViewLogic;
            _currentChainViewLogic.OnViewEnter();
        }
        public void SetRestingOnFloorView()
        {
            _restingOnFloorChainViewLogic.EnterSetup(_currentChainViewLogic.GetChainPositions(), 
                _playerBindTransform.position, _anchorBindTransform.position);
            _currentChainViewLogic.OnViewExit();
            _currentChainViewLogic = _restingOnFloorChainViewLogic;
            _currentChainViewLogic.OnViewEnter();
        }
        public void SetCarriedView()
        {
            _chainLine.enabled = true;
            _currentChainViewLogic.OnViewExit();
            _currentChainViewLogic = _carriedChainViewLogic;
            _currentChainViewLogic.OnViewEnter();
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