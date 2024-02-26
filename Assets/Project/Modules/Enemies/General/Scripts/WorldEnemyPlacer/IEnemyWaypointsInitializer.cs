using Popeye.Modules.Enemies;

namespace Project.Modules.Enemies.General
{
    public interface IEnemyWaypointsInitializer
    {
        void SetEnemyWaypoints(AEnemy enemy);
    }
}