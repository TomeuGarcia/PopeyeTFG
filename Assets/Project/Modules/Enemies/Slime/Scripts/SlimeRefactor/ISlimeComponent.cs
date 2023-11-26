using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISlimeComponent : MonoBehaviour
{
    protected SlimeMediator _mediator;

    public void Configure(SlimeMediator slimeMediator)
    {
        _mediator = slimeMediator;
    }
}
