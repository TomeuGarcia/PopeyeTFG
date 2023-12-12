using System;
using DG.Tweening;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Chain
{
    public class SingleSpringChainPhysics : MonoBehaviour, IChainPhysics
    {
        [SerializeField] private SpringJoint _springJoint;
        private ChainConfig _chainConfig = null;

        private bool _isTensionEnabled;
        private bool _failedThrow;

        private void OnEnable()
        {
            SubscribeToEvents();
        }
        private void OnDisable()
        {
            UnsubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            if (_chainConfig != null)
            {
                _chainConfig.OnValuesUpdated += OnChainConfigValuesUpdated;
            }
        }
        private void UnsubscribeToEvents()
        {
            if (_chainConfig != null)
            {
                _chainConfig.OnValuesUpdated -= OnChainConfigValuesUpdated;
            }
        }
        

        public void Configure(ChainConfig chainConfig)
        {
            _chainConfig = chainConfig;

            UnsubscribeToEvents();
            SubscribeToEvents();

            EnableTension();
        }

        public void SetFailedThrow(bool failedThrow)
        {
            _failedThrow = failedThrow;
        }

        private void OnChainConfigValuesUpdated()
        {
            if (_isTensionEnabled)
            {
                SetSpringJointMaxDistance(GetAdequateChainLength());
                SetSpringJointForce(_chainConfig.ChainSpringForce);
            }
        }


        public void EnableTension()
        {
            SetSpringJointMaxDistance(GetAdequateChainLength());
            _isTensionEnabled = true;
        }

        public void DisableTension()
        {
            SetSpringJointMaxDistance(1000);
            _isTensionEnabled = true;
        }


        private float GetAdequateChainLength()
        {
            return _failedThrow
                ? _chainConfig.MaxChainLength + _chainConfig.FailedThrowExtraLength
                : _chainConfig.MaxChainLength;
        }

        private void SetSpringJointMaxDistance(float maxDistance)
        {
            _springJoint.maxDistance = maxDistance;
        }
        private void SetSpringJointForce(float force)
        {
            float currentForce = _springJoint.spring;
            
            DOTween.To(
                () => currentForce,
                (value) => _springJoint.spring = value,
                force,
                0.2f
            );
            
        }
    }
}