using Popeye.Core.Services.ServiceLocator;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops
{
    [CreateAssetMenu(fileName = "PowerBoostDropper_NAME", 
        menuName = ScriptableObjectsHelper.PLAYERPOWERBOOSTDROPS_ASSETS_PATH + "PowerBoostDropper")]
    public class PowerBoostDropper : ScriptableObject
    {
        [SerializeField] private PowerBoostDropConfig _dropConfig;
        private IPowerBoostDropFactory _dropFactory = null;
        
        
        public void SpawnDrop(Vector3 position)
        {
            if (_dropFactory == null)
            {
                _dropFactory = ServiceLocator.Instance.GetService<IPowerBoostDropFactory>();
            }
            
            _dropFactory.Create(position, Quaternion.identity, _dropConfig);
        }
    }
}