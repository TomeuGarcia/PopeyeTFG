using System;
using UnityEngine;

namespace Popeye.Modules.VFX.Anchor.Throw
{
    public class AnchorThrowHeadFollower : MonoBehaviour
    {
        [Header("REFERENCES")] 
        [SerializeField] private Transform _anchorMoveRotate;
        
        private float _fixedHeight;

        private void Awake()
        {
            StopFollowing();
        }

        public void StartFollowing()
        {
            Debug.Log(gameObject.activeInHierarchy);
            gameObject.SetActive(true);
            _fixedHeight = _anchorMoveRotate.position.y;
        }

        public void StopFollowing()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            UpdateFixedHeight();
        }

        private void UpdateFixedHeight()
        {
            transform.position = new Vector3(_anchorMoveRotate.position.x, _fixedHeight, _anchorMoveRotate.position.z);
        }
    }
}