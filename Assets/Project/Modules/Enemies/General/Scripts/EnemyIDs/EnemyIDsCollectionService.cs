
namespace Popeye.Modules.Enemies.General
{
    public class EnemyIDsCollectionService : IEnemyIDsCollectionService
    {
        public EnemyID[] EnemyIDs { get; }

        public EnemyIDsCollectionService(EnemyID[] ids)
        {
            EnemyIDs = ids;
        }

    }
}