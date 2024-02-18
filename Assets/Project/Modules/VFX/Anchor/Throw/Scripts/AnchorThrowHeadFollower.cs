using System;
using UnityEngine;

namespace Project.Modules.VFX.Anchor.Throw.Scripts
{
    public class AnchorThrowHeadFollower : MonoBehaviour
    {
        [Header("REFERENCES")] 
        [SerializeField] private Transform _anchorMoveRotate;
        
        private float _fixedHeight;

        public void OnEnable()
        {
            _fixedHeight = _anchorMoveRotate.position.y;
        }

        private void Update()
        {
            transform.position = new Vector3(_anchorMoveRotate.position.x, _fixedHeight, _anchorMoveRotate.position.z);
        }
    }
}