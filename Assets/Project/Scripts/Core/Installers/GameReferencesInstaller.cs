using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using UnityEngine;

namespace Popeye.Core.Installers
{
    public class GameReferencesInstaller : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;

        void Awake()
        {
            ServiceLocator.Instance.RegisterService<IGameReferences>(new GameReferences(_playerTransform));
        }


    }
}
