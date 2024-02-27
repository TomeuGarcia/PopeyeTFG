using Popeye.Modules.Enemies;
using Popeye.Modules.Enemies.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Modules.Enemies.General
{
    public class EnemyWaypointsInitializerBehaviour : MonoBehaviour, IEnemyWaypointsInitializer
    { 
       [SerializeField] private EnemyPatrolling.PatrolType _patrolType;
       [SerializeField] private Transform[] _wayPoints;
        public void SetEnemyWaypoints(AEnemy enemy)
        {
            if (_patrolType == EnemyPatrolling.PatrolType.FixedWaypoints)
            {
                enemy.SetPatrollingWaypoints(_wayPoints);
            }
            
            
        }
        
    }
}