using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerController.Inputs;
using Popeye.Modules.PlayerController.LookRotation;
using Popeye.Scripts.Collisions;
using UnityEngine;



using UnityEngine.Serialization;


namespace Popeye.Modules.PlayerController
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerControllerUpdater : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;
    
        private void Update()
        {
            _playerController.DoUpdate();
        }
        private void FixedUpdate()
        {
            _playerController.DoFixedUpdate();
        }
        
    }
}