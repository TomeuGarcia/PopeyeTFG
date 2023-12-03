using System;
using Cysharp.Threading.Tasks;
using Project.Modules.CombatSystem;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class AnchorDamageDealer : MonoBehaviour
    {
        private AnchorDamageConfig _config;
        [SerializeField] private DamageTrigger _anchorThrowDamageTrigger;
        
        
        public void Configure(AnchorDamageConfig anchorDamageConfig, ICombatManager combatManager)
        {
            _config = anchorDamageConfig;
            _anchorThrowDamageTrigger.Configure(combatManager, new DamageHit(_config.AnchorThrowDamageHit));
            _anchorThrowDamageTrigger.Deactivate();
        }


        public async UniTaskVoid DealThrowDamage(float throwDuration)
        {
            _anchorThrowDamageTrigger.Activate();
            await UniTask.Delay(TimeSpan.FromSeconds(throwDuration));
            _anchorThrowDamageTrigger.Deactivate();
        }
        
        
    }
}