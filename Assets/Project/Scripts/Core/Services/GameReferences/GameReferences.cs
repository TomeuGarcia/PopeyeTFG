using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Core.Services.GameReferences
{
   public class GameReferences : IGameReferences
   {
      private Transform _playerTransform;


      public GameReferences(Transform playerTransform)
      {
         SetPlayer(playerTransform);
      }
      public Transform GetPlayer()
      {
         return _playerTransform;
      }

      public void SetPlayer(Transform playerTransform)
      {
         _playerTransform = playerTransform;
      }
   }
}
