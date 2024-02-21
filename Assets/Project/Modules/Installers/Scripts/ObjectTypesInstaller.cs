using Popeye.Core.Services.ServiceLocator;
using Popeye.Scripts.ObjectTypes;
using UnityEngine;

namespace Project.Modules.Installers.Scripts
{
    public class ObjectTypesInstaller : MonoBehaviour
    {
        [SerializeField] private ObjectTypeAsset _playerObjectType;
        
        
        public void Install()
        {
            ServiceLocator.Instance.RegisterService<IObjectTypesGameService>(
                new ObjectTypesGameService(_playerObjectType)
                );
        }

        public void Uninstall()
        {
            ServiceLocator.Instance.RemoveService<IObjectTypesGameService>();
        }
    }
}