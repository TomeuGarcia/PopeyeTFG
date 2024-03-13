using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Core.Services.GameReferences
{
    public interface IGameReferences
    {
        Transform GetPlayerTargetForEnemies();
        
    }
}
