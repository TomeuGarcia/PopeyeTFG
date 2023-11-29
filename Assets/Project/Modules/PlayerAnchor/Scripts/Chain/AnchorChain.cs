using System;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Chain
{
    public class AnchorChain : MonoBehaviour
    {
        private Transform _playerBindTransform;
        private Transform _anchorBindTransform;
        [SerializeField] private LineRenderer _chainLine;
        
        
        public void Configure(Transform playerBindTransform, Transform anchorBindTransform)
        {
            _playerBindTransform = playerBindTransform;
            _anchorBindTransform = anchorBindTransform;
        }


        private void Update()
        {
            _chainLine.SetPosition(0, _playerBindTransform.position);
            _chainLine.SetPosition(1, _anchorBindTransform.position);
        }

        public void Show()
        {
            _chainLine.enabled = true;
        }
        public void Hide()
        {
            _chainLine.enabled = false;
        }
        
    }
}