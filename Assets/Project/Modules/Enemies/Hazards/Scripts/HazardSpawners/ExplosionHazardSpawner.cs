using System;
using Popeye.Core.Services.ServiceLocator;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public class ExplosionHazardSpawner : MonoBehaviour, IHazardSpawner
    {
        [SerializeField] private ExplosionSize _size = ExplosionSize.Small;
        
        private IHazardFactory _hazardFactory;
        
        private void Start()
        {
            _hazardFactory = ServiceLocator.Instance.GetService<IHazardFactory>();
        }

        public void Spawn(Vector3 position, Quaternion rotation)
        {
            _hazardFactory.CreateExplosion(position, rotation, _size);
        }
        
        public void SetExplosionSize(ExplosionSize size)
        {
            _size = size;
        }
    }
}