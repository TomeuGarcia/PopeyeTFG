using System;
using Popeye.Core.Pool;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops
{
    public class AutoCollectPowerBoostDrop : RecyclableObject, IPowerBoostDrop
    {
        public int Experience { get; private set; }
        private bool _wasUsed;

        private Transform _defaultParent;

        [SerializeField] private ParticleSystem _particleSystem;

        private ParticleSystem.ShapeModule _shapeModule;

        private void Awake()
        {
            _shapeModule = _particleSystem.shape;
            _defaultParent = transform.parent;
        }

        public void Init(int experience, Transform autoCollectTransform)
        {
            Experience = experience;

            Transform particleTransform = transform;
            
            _shapeModule.position = particleTransform.position - autoCollectTransform.position;
                
            particleTransform.parent = autoCollectTransform;
            particleTransform.localPosition = Vector3.zero;
            
            _particleSystem.Play();
        }

        private void OnParticleSystemStopped()
        {
            Recycle();
        }


        internal override void Init()
        {
            _wasUsed = false;
        }

        internal override void Release()
        {
            transform.parent = _defaultParent;
        }

        public bool CanBeUsed()
        {
            return !_wasUsed;
        }

        public int GetExperienceAndSetUsed()
        {
            _wasUsed = true;
            return Experience;
        }
    }
}