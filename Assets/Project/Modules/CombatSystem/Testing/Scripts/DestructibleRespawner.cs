using Popeye.Modules.WorldElements.WorldInteractors;
using UnityEngine;

namespace Popeye.Modules.CombatSystem.Testing.Scripts
{
    public class DestructibleRespawner : AWorldInteractor
    {
        private DestructibleProp[] _destructibleProps;

        protected override void AwakeInit()
        {
            _destructibleProps = new DestructibleProp[transform.childCount];
            for (int i = 0; i < transform.childCount; ++i)
            {
                _destructibleProps[i] = transform.GetChild(i).GetComponent<DestructibleProp>();
            }
        }

        protected override void EnterActivatedState()
        {
            RespawnDestructibleProps();
        }

        protected override void EnterDeactivatedState()
        {
            
        }

        private void RespawnDestructibleProps()
        {
            foreach (DestructibleProp destructibleProp in _destructibleProps)
            {
                destructibleProp.Spawn();
            }
        }
    }
}