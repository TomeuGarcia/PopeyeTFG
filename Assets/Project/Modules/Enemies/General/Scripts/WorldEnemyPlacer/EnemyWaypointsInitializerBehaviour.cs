using Popeye.Modules.Enemies;
using Popeye.Modules.Enemies.Components;
using UnityEngine;

namespace Project.Modules.Enemies.General
{
    public class EnemyWaypointsInitializerBehaviour : MonoBehaviour, IEnemyWaypointsInitializer
    { 
       [SerializeField] private EnemyPatrolling.PatrolType patrolType;
       [SerializeField] private Transform[] wayPoints;
        public void SetEnemyWaypoints(AEnemy enemy)
        {
            enemy.SetPatrollingWaypoints(wayPoints);
        }

        public Transform[] GetWayPoints()
        {
            return wayPoints;
        }

        public EnemyPatrolling.PatrolType GetPatrolType()
        {
            return patrolType;
        }
    }
}