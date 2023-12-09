using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies.Components
{
    public class SlimeDivider : MonoBehaviour
    {

        [SerializeField] private GameObject _slimeToSpawn;
        [SerializeField] private int _numberOfSlimes;

        private SlimeMediator _mediator;


        public void Configure(SlimeMediator slimeMediator)
        {
            _mediator = slimeMediator;
        }

        public void SpawnSlimes()
        {
            var tempTransform = transform;

            for (int i = 0; i < _numberOfSlimes; i++)
            {
                float angle = i * 360f / _numberOfSlimes;
                Vector3 dir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

                GameObject slimeSpawned = Instantiate(_slimeToSpawn, tempTransform.position, Quaternion.identity,
                    tempTransform.parent.parent);

                SlimeMediator childSlimeMediator = slimeSpawned.GetComponent<SlimeMediator>();
                childSlimeMediator.Init();
                childSlimeMediator.SetSlimeMind(_mediator.slimeMindEnemy);
                childSlimeMediator.SetPlayerTransform(_mediator.playerTransform);
                childSlimeMediator.SpawningFromDivision(dir,_mediator.GetPatrolType(),_mediator.GetPatrolWaypoints());
                
                _mediator.AddSlimesToSlimeMindList(childSlimeMediator);
            }
        }


    }
}
