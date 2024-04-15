using System;
using Popeye.Core.Services.ServiceLocator;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public abstract class AHazardSpawner : MonoBehaviour
    {
        protected IHazardFactory HazardFactory { get; private set; }
        
        private void Start()
        {
            HazardFactory = ServiceLocator.Instance.GetService<IHazardFactory>();
        }

        public abstract void Spawn(Vector3 position, Quaternion rotation);
    }
}