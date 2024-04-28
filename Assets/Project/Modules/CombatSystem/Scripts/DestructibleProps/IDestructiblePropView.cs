using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.CombatSystem.Testing.Scripts
{
    public interface IDestructiblePropView
    {
        void Configure(DestructiblePropConfig.ViewConfigData config, Transform viewHolder);
        void PlayTakeDamageAnimation();
        UniTask PlayDestroyedAnimation();
    }
}