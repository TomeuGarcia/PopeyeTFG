using Popeye.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.InstantTranslation
{
    public class PopeyePlayerInstantTranslation : IPlayerInstantTranslation
    {
        private readonly PlayerController.PlayerController _playerController;
        private readonly TransformMotion _playerMotion;
        private readonly TransformMotion _anchorMotion;
        private readonly IAnchorMediator _anchorMediator;

        public PopeyePlayerInstantTranslation(
            PlayerController.PlayerController playerController, 
            TransformMotion playerMotion,
            IAnchorMediator anchorMediator,
            TransformMotion anchorMotion)
        {
            _playerController = playerController;
            _playerMotion = playerMotion;
            _anchorMotion = anchorMotion;
            _anchorMediator = anchorMediator;
        }
        
        
        public void TranslatePlayer(Vector3 position, Quaternion rotation)
        {
            _playerController.ResetRigidbody();
            _playerMotion.SetPosition(position);  
            _playerMotion.SetRotation(rotation);
        }

        public void TranslatePlayerAndPauseForAFrame(Vector3 position, Quaternion rotation)
        {
            TranslatePlayer(position, rotation);
            _playerController.DisableForDuration(Time.deltaTime).Forget();
        }

        public void TranslateAnchorAndReset(Vector3 position, Quaternion rotation)
        {
            _anchorMediator.ResetState(position); 
            _anchorMotion.SetRotation(rotation);
        }


    }
}