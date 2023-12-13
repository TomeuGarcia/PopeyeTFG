using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies.StateMachine
{
    public class EnemyDeadState : IEnemyState
    {
        private Enemy _enemy;

        public EnemyDeadState(Enemy enemy)
        {
            _enemy = enemy;
        }


        protected override void DoEnter()
        {
            StartDeathAnimation();
        }

        public override void Exit()
        {

        }

        public override void Interrupt()
        {

        }

        public override bool Update(float deltaTime)
        {
            return false;
        }

        private void StartDeathAnimation()
        {
            _enemy.StartDeathAnimation().Forget();
        }

    }
}
