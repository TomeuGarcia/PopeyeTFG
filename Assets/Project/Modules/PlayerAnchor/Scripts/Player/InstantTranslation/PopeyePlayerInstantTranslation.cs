using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.InstantTranslation
{
    public class PopeyePlayerInstantTranslation : IPlayerInstantTranslation
    {
        private readonly PlayerController.PlayerController _playerController;
        private readonly TransformMotion _playerMotion;
        private readonly TransformMotion _anchorMotion;

        public PopeyePlayerInstantTranslation(
            PlayerController.PlayerController playerController, 
            TransformMotion playerMotion,
            TransformMotion anchorMotion)
        {
            _playerController = playerController;
            _playerMotion = playerMotion;
            _anchorMotion = anchorMotion;
        }
        
        
        public void TranslatePlayer(Vector3 position, Quaternion rotation)
        {
            _playerController.ResetRigidbody();
            _playerMotion.SetPosition(position);  
            _playerMotion.SetRotation(rotation);
        }

        public void TranslateAnchor(Vector3 position, Quaternion rotation)
        {
            _anchorMotion.SetPosition(position);  
            _anchorMotion.SetRotation(rotation);
        }


    }
}