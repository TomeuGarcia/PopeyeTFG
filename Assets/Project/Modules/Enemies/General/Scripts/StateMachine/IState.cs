using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies.StateMachine
{
   public interface IState
   {
      IState ProcessTransitions();

      void Enter();

      void Exit();

   }
}
