using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.CombatSystem.Testing.Scripts
{
    public class DestructiblePropView : MonoBehaviour, IDestructiblePropView
    {
        private DestructiblePropConfig.ViewConfigData _config;
        [SerializeField] private Transform _mesh;
        
        
        public void Configure(DestructiblePropConfig.ViewConfigData config, Transform viewHolder)
        {
            _config = config;
        }

        public void PlayTakeDamageAnimation()
        {
            
        }

        public async UniTask PlayDestroyedAnimation()
        {            
            _mesh.gameObject.SetActive(false);
        }

    }
}