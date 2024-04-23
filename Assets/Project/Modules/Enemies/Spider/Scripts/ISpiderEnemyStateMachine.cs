using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies.StateMachine
{
    public abstract class ISpiderEnemyStateMachine : MonoBehaviour
    {

        public abstract void AwakeInit(SpiderEnemy spiderEnemy);
        public abstract void ResetStateMachine();
        public abstract void OverwriteCurrentState(ISpiderEnemyState.States newState);


    }
}