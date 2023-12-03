using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Core.Services.GameReferences
{
   public class GameReferences : IGameReferences
   {
      public GameReferences(Transform playerTransform)
      {
         _playerTransform = playerTransform;
      }
   }
}
