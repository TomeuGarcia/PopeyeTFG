using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.PlayerAnchor.Player;
using UnityEngine;

namespace Popeye.Core.Installers
{
    public class GameReferencesInstaller : MonoBehaviour
    {
        public void Install(ServiceLocator serviceLocator, IPlayerMediator playerMediator)
        {
            GameReferences gameReferences = new GameReferences(
                playerMediator.GetDeathNotifier(),
                playerMediator.GetTargetForEnemies(),
                playerMediator.GetPlayerEnemySpawnersInteractions()
            );
            
            serviceLocator.RegisterService<IGameReferences>(gameReferences);
        }

        public void Uninstall(ServiceLocator serviceLocator)
        {
            serviceLocator.RemoveService<IGameReferences>();
        }
        

    }
}
