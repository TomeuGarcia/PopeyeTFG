using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public class ExplosionHazardSpawner : AHazardSpawner
    {
        [SerializeField] private ExplosionSize _size = ExplosionSize.Small;
        
        public override void Spawn(Vector3 position, Quaternion rotation)
        {
            HazardFactory.CreateExplosion(position, rotation, _size);
        }
        
        public void SetExplosionSize(ExplosionSize size)
        {
            _size = size;
        }
    }
}