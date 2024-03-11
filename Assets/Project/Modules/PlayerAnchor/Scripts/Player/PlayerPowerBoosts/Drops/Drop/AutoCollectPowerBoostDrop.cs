using Popeye.Core.Pool;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops
{
    public class AutoCollectPowerBoostDrop : RecyclableObject, IPowerBoostDrop
    {
        public int Experience { get; private set; }

        [SerializeField] private ParticleSystem _particleSystem;
        
        
        public void Init(int experience, Transform autoCollectTransform)
        {
            Experience = experience;

            transform.parent = autoCollectTransform;
            transform.localPosition = Vector3.zero;
            
            
        }

        
        internal override void Init()
        {
            
        }

        internal override void Release()
        {
            
        }
    }
}