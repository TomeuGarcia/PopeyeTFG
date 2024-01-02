using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies.StateMachine
{
    public class SpiderSpiderEnemyDeadState : ISpiderEnemyState
    {
        private SpiderEnemy _spiderEnemy;

        public SpiderSpiderEnemyDeadState(SpiderEnemy spiderEnemy)
        {
            _spiderEnemy = spiderEnemy;
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
            _spiderEnemy.StartDeathAnimation().Forget();
        }

    }
}
