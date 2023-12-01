using System;
using AYellowpaper;
using Popeye.Modules.PlayerAnchor.Player;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using Popeye.Modules.PlayerController;
using Popeye.Modules.PlayerController.Inputs;
using Popeye.Modules.Camera;
using Project.Modules.PlayerAnchor.Anchor;
using Project.Modules.PlayerAnchor.Anchor.AnchorStates;
using Project.Modules.PlayerAnchor.Chain;
using UnityEngine;

namespace Project.Modules.PlayerAnchor
{
    public class PlayerAnchorInstaller : MonoBehaviour
    {
        [Header("CAMERA")] 
        [SerializeField] private OrbitingCamera _isometricCamera;
        
        
        [Space(20)]
        [Header("PLAYER")]
        [SerializeField] private PopeyePlayer _player;
        [SerializeField] private PlayerController _playerController;

        [Header("Player States")] 
        [SerializeField] private PlayerStatesConfig _playerStatesConfigurations;

        
        [Space(20)] 
        [Header("ANCHOR")] 
        [SerializeField] private PopeyeAnchor _anchor;
        [SerializeField] private AnchorPhysics _anchorPhysics;
        [SerializeField] private Transform _anchorMoveTransform;
        
        [Header("Anchor Throwing")]
        [SerializeField] private AnchorThrowConfig _anchorThrowConfig;
        [SerializeField] private AnchorPullConfig _anchorPullConfig;
        [SerializeField] private AnchorTrajectoryEndSpot _anchorTrajectoryEndSpot;
        
        
        [Header("CHAIN")]
        [SerializeField] private ChainConfig _anchorChainConfig;
        [SerializeField] private AnchorChain _anchorChain;
        [SerializeField] private InterfaceReference<IChainPhysics, MonoBehaviour> _chainPhysics;

        [SerializeField] private Transform _chainPlayerBindTransform;
        [SerializeField] private Transform _chainAnchorBindTransform;

        [SerializeField] private LineRenderer debugLine;
        [SerializeField] private LineRenderer debugLine2;
        [SerializeField] private LineRenderer debugLine3;

        private void Awake()
        {
            Install();
        }

        private void Install()
        {
            // Anchor
            AnchorMotion anchorMotion = new AnchorMotion();
            AnchorThrower anchorThrower = new AnchorThrower();
            AnchorPuller anchorPuller = new AnchorPuller();
            AnchorThrowTrajectory anchorThrowTrajectory = new AnchorThrowTrajectory();
            AnchorStatesBlackboard anchorStatesBlackboard = new AnchorStatesBlackboard();
            AnchorFSM anchorStateMachine = new AnchorFSM();
            IChainPhysics chainPhysics = _chainPhysics.Value;
            
            
            anchorMotion.Configure(_anchorMoveTransform);
            anchorThrower.Configure(_player, _anchor, anchorThrowTrajectory, anchorMotion, _anchorThrowConfig);
            anchorPuller.Configure(_player, _anchor, anchorThrowTrajectory, anchorMotion, _anchorPullConfig);
            anchorThrowTrajectory.Configure(_anchorTrajectoryEndSpot, _anchorThrowConfig, _anchorPullConfig,
                debugLine, debugLine2, debugLine3);
            anchorStatesBlackboard.Configure(anchorMotion, _anchorPhysics, _anchorChain, _player.AnchorCarryHolder, 
                _player.AnchorGrabToThrowHolder);
            anchorStateMachine.Setup(anchorStatesBlackboard);
            chainPhysics.Configure(_anchorChainConfig);

            _anchor.Configure(anchorStateMachine, anchorThrowTrajectory, anchorThrower, anchorPuller, anchorMotion,
                _anchorChain);
            _anchorPhysics.Configure(_anchor);
            _anchorChain.Configure(chainPhysics, _chainPlayerBindTransform, _chainAnchorBindTransform);

            
            
            // Player
            IMovementInputHandler movementInputHandler = new CameraAxisMovementInput(_isometricCamera.CameraTransform);
            PlayerAnchorMovesetInputsController movesetInputsController = new PlayerAnchorMovesetInputsController();
            PlayerStatesBlackboard playerStatesBlackboard = new PlayerStatesBlackboard();
            PlayerFSM playerStateMachine = new PlayerFSM();
            
            
            playerStatesBlackboard.Configure(_playerStatesConfigurations, _player, movesetInputsController, _anchor, 
                anchorThrower, anchorPuller);
            playerStateMachine.Setup(playerStatesBlackboard);

            _player.Configure(playerStateMachine, _playerController, _anchor);
            _playerController.MovementInputHandler = movementInputHandler;
        }
    }
}