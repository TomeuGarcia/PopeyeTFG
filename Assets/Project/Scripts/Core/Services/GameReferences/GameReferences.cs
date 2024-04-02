using UnityEngine;

namespace Popeye.Core.Services.GameReferences
{
   public class GameReferences : IGameReferences
   {
      private readonly Transform _playerTargetForEnemies;
      private readonly Transform _playerPosition;


      public GameReferences(Transform playerTargetForEnemies, Transform playerPosition)
      {
         _playerTargetForEnemies = playerTargetForEnemies;
         _playerPosition = playerPosition;
      }
      
      public Transform GetPlayerTargetForEnemies()
      {
         return _playerTargetForEnemies;
      }

      public Transform GetPlayerPositionTransform()
      {
         return _playerPosition;
      }
   }
}
