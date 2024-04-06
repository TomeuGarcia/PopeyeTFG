using UnityEngine;

namespace Project.Modules.WorldElements.DestructiblePlatforms
{
    public class DestructiblePlatformCollider
    {
        private readonly Collider[] _colliders;

        public DestructiblePlatformCollider(Collider[] colliders)
        {
            _colliders = colliders;
            EnableCollisions();
        }

        public void EnableCollisions()
        {
            foreach (Collider collider in _colliders)
            {
                collider.enabled = true;
            }
        }
        
        public void DisableCollisions()
        {
            foreach (Collider collider in _colliders)
            {
                collider.enabled = false;
            }
        }
        
    }
}