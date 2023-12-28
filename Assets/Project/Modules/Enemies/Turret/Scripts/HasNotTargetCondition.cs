using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies.StateMachine
{
    public class HasNotTargetCondition : StateTransitionCondition
    {
        [SerializeField] private ProximityTargetGetterBehaviour _targetGetterBehaviour = null;
        [SerializeField] private bool _shouldHaveTarget = true;

        public override bool IsMet() => _targetGetterBehaviour.HasTarget == _shouldHaveTarget;
    }
}
