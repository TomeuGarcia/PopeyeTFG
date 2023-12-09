using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies.StateMachine
{
    public abstract class StateTransitionCondition : MonoBehaviour
    {
        public abstract bool IsMet();
    }
}
