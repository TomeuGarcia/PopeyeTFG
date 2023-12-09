using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMediator
{

    public void OnDeath();
    public void OnPlayerClose();
    public void OnPlayerFar();
    public void OnHit();
}
