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
        
        private IChainView _currentChainView;
        private SpiralThrowChainView _thrownChainView;
        private SpiralThrowChainView _pullChainView;
        private HangingPhysicsChainView _restingOnFloorChainView;
        private IChainView _carriedChainView;

        
        public void Configure(IChainPhysics chainPhysics, Transform playerBindTransform, Transform anchorBindTransform,
            ChainViewGeneralConfig chainViewGeneralConfig)
        {
            _chainPhysics = chainPhysics;
            _playerBindTransform = playerBindTransform;
            _anchorBindTransform = anchorBindTransform;

            _thrownChainView = new SpiralThrowChainView(_chainLine, chainViewGeneralConfig.ThrowViewConfig, chainViewGeneralConfig.ChainBoneCount);
            _pullChainView = new SpiralThrowChainView(_chainLine, chainViewGeneralConfig.PullViewConfig, chainViewGeneralConfig.ChainBoneCount);
            _restingOnFloorChainView = new HangingPhysicsChainView(_chainLine, chainViewGeneralConfig.RestingOnFloorViewConfig, 
                chainViewGeneralConfig.ChainBoneCount, _chainIK, _boneChainIK);
            _carriedChainView = new StraightLineChainView(_chainLine);
            _currentChainView = _carriedChainView;
        }


        private void LateUpdate()
        {
            _currentChainView.LateUpdate(Time.deltaTime, _playerBindTransform.position, _anchorBindTransform.position);
        }

        public void SetThrownView(AnchorThrowResult throwResult)
        {
            _currentChainView.OnViewExit();
            _thrownChainView.EnterSetup(throwResult);
            _currentChainView = _thrownChainView;
            _currentChainView.OnViewEnter();
        }
        public void SetPulledView(AnchorThrowResult pullResult)
        {
            _pullChainView.EnterSetup(pullResult);
            _currentChainView.OnViewExit();
            _currentChainView = _pullChainView;
            _currentChainView.OnViewEnter();
        }
        public void SetRestingOnFloorView()
        {
            _currentChainView.OnViewExit();
            _currentChainView = _restingOnFloorChainView;
            _currentChainView.OnViewEnter();
        }
        public void SetCarriedView()
        {
            _chainLine.enabled = true;
            _currentChainView.OnViewExit();
            _currentChainView = _carriedChainView;
            _currentChainView.OnViewEnter();
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