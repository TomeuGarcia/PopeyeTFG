using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{

    public abstract class APlayerState
    {
        public PlayerStates NextState { get; protected set; }

        public virtual bool HasSubState(PlayerStates state)
        {
            return false;
            
        }

        public void Enter()
        {
            NextState = PlayerStates.None;
            DoEnter();
        }

        protected abstract void DoEnter();
        public abstract void Exit();
        public abstract bool Update(float deltaTime);
    }


}