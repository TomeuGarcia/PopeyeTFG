using UnityEngine;

namespace Popeye.Core.Services.GameReferences
{
   public class GameReferences : IGameReferences
   {
      private readonly Transform _playerTargetForEnemies;


      public GameReferences(Transform playerTargetForEnemies)
      {
         _playerTargetForEnemies = playerTargetForEnemies;
      }
      
      public Transform GetPlayerTargetForEnemies()
      {
         return _playerTargetForEnemies;
      }
      


   }
}
