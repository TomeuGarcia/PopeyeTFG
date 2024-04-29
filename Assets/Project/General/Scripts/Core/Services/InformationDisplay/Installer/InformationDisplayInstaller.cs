using UnityEngine;

namespace Popeye.Core.Services.InformationDisplay
{
    public class InformationDisplayInstaller : MonoBehaviour
    {
        [SerializeField] private CanvasTextDisplayer _canvasTextDisplayer;
        [SerializeField] private CanvasVideoDisplayer _canvasVideoDisplayer;
        
        public void Install(ServiceLocator.ServiceLocator serviceLocator)
        {
            InformationDisplayService informationDisplayService = 
                new InformationDisplayService(_canvasTextDisplayer, _canvasVideoDisplayer);
            
            serviceLocator.RegisterService<IInformationDisplayService>(informationDisplayService);
        }

        public void Uninstall(ServiceLocator.ServiceLocator serviceLocator)
        {
            serviceLocator.RemoveService<IInformationDisplayService>();
        }
    }
}