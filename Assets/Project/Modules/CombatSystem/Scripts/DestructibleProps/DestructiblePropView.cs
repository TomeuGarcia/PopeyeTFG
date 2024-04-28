using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.CombatSystem.Testing.Scripts
{
    public class DestructiblePropView : MonoBehaviour, IDestructiblePropView
    {
        private DestructiblePropConfig.ViewConfigData _config;
        private Transform _viewHolder;
        
        
        public void Configure(DestructiblePropConfig.ViewConfigData config, Transform viewHolder)
        {
            _config = config;
            _viewHolder = viewHolder;
        }

        public void PlayTakeDamageAnimation()
        {
            _viewHolder.PunchScale(_config.TakeDamagePunchScale);
        }

        public async UniTask PlayDestroyedAnimation()
        {            
            _viewHolder.PunchScale(_config.DeathPunchScale);
            _viewHolder.RotateBy(_config.DeathRotation);

            float waitDuration = Mathf.Max(_config.DeathPunchScale.Duration, _config.DeathRotation.Duration);
            await UniTask.Delay(TimeSpan.FromSeconds(waitDuration));
        }

    }
}