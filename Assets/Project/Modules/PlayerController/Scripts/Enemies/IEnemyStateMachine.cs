using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies.StateMachine
{
    public abstract class IEnemyStateMachine : MonoBehaviour
    {

        public abstract void AwakeInit(Enemy enemy);
        public abstract void ResetStateMachine();
        public abstract void OverwriteCurrentState(IEnemyState.States newState);


    }
}