using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.PlayerAnchor.Player;
using Popeye.Modules.PlayerAnchor.Player.DeathDelegate;
using Popeye.Modules.PlayerAnchor.Player.EnemyInteractions;
using UnityEngine;

namespace Popeye.Core.Services.GameReferences
{
   public class GameReferences : IGameReferences
   {
      private readonly IPlayerDeathNotifier _playerDeathNotifier;
      private readonly Transform _playerTargetForEnemies;
      private readonly IPlayerEnemySpawnersInteractions _playerEnemySpawnersInteractions;


      public GameReferences(IPlayerDeathNotifier playerDeathNotifier, Transform playerTargetForEnemies,
         IPlayerEnemySpawnersInteractions playerEnemySpawnersInteractions)
      {
         _playerDeathNotifier = playerDeathNotifier;
         _playerTargetForEnemies = playerTargetForEnemies;
         _playerEnemySpawnersInteractions = playerEnemySpawnersInteractions;
      }
      
      public Transform GetPlayerTargetForEnemies()
      {
         return _playerTargetForEnemies;
      }

      public IPlayerDeathNotifier GetPlayerDeathNotifier()
      {
         return _playerDeathNotifier;
      }

      public IPlayerEnemySpawnersInteractions GetPlayerEnemySpawnersInteractions()
      {
         return _playerEnemySpawnersInteractions;
      }
   }
}
