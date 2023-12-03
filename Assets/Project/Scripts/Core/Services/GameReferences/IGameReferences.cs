using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Core.Services.GameReferences
{
    public class IGameReferences : MonoBehaviour
    {
        protected Transform _playerTransform;

        public Transform GetPlayer()
        {
            return _playerTransform;
        }

    }
}
