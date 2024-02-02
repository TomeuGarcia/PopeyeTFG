using System;
using Popeye.Modules.PlayerAnchor.Player;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class AnchorChain : MonoBehaviour
    {
        [SerializeField] private LineRenderer _chainLine;
        private Transform _playerBindTransform;
        private Transform _anchorBindTransform;
        
        private IChainPhysics _chainPhysics;
        
        private IChainView _currentChainView;
        private SpiralThrowChainView _thrownChainView;
        private IChainView _carriedChainView;

        
        public void Configure(IChainPhysics chainPhysics, Transform playerBindTransform, Transform anchorBindTransform,
            SpiralThrowChainViewConfig spiralThrowChainViewConfig)
        {
            _chainPhysics = chainPhysics;
            _playerBindTransform = playerBindTransform;
            _anchorBindTransform = anchorBindTransform;

            _thrownChainView = new SpiralThrowChainView(_chainLine, spiralThrowChainViewConfig);
            _carriedChainView = new StraightLineChainView(_chainLine);
            _currentChainView = _carriedChainView;
        }


        private void LateUpdate()
        {
            _currentChainView.LateUpdate(Time.deltaTime, _playerBindTransform.position, _anchorBindTransform.position);
        }

        public void SetThrownView(AnchorThrowResult throwResult)
        {
            _thrownChainView.Setup(throwResult);
            _currentChainView.OnViewSwapped();
            _currentChainView = _thrownChainView;
        }
        public void SetCarriedView()
        {
            _chainLine.enabled = true;
            _currentChainView.OnViewSwapped();
            _currentChainView = _carriedChainView;
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