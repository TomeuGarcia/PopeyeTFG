using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.PlayerAnchor.Player.DeathDelegate;
using Popeye.Modules.PlayerAnchor.Player.EnemyInteractions;
using UnityEngine;

namespace Popeye.Core.Services.GameReferences
{
    public interface IGameReferences
    {
        Transform GetPlayerTargetForEnemies();
        IPlayerDeathNotifier GetPlayerDeathNotifier();
        IPlayerEnemySpawnersInteractions GetPlayerEnemySpawnersInteractions();
        
    }
}
