using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.PlayerAnchor.Player;
using Popeye.Modules.PlayerAnchor.Player.DeathDelegate;
using UnityEngine;

namespace Popeye.Core.Services.GameReferences
{
   public class GameReferences : IGameReferences
   {
      private readonly IPlayerDeathNotifier _playerDeathNotifier;
      private readonly Transform _playerTargetForEnemies;


      public GameReferences(IPlayerDeathNotifier playerDeathNotifier, Transform playerTargetForEnemies)
      {
         _playerDeathNotifier = playerDeathNotifier;
         _playerTargetForEnemies = playerTargetForEnemies;
      }
      
      public Transform GetPlayerTargetForEnemies()
      {
         return _playerTargetForEnemies;
      }

      public IPlayerDeathNotifier GetPlayerDeathNotifier()
      {
         return _playerDeathNotifier;
      }
   }
}
