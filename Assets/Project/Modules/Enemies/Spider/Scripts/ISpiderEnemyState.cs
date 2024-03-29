using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISpiderEnemyState
{
    public enum States
    {
        None,
        Idle,
        Chasing,
        Dashing,
        Stunned,
        Dead
    }

    protected States _nextState;
    public States NextState => _nextState;


    public void Enter()
    {
        _nextState = States.None;
        DoEnter();
    }

    protected abstract void DoEnter();
    public abstract void Exit();
    public abstract void Interrupt();
    public abstract bool Update(float deltaTime);
}
