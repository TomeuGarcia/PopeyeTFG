using System;
using AYellowpaper;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.PlayerAnchor.Player;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using Popeye.Modules.PlayerController;
using Popeye.Modules.PlayerController.Inputs;
using Popeye.Modules.Camera;
using Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations;
using Popeye.Modules.ValueStatSystem;
using Project.Modules.CombatSystem;
using Project.Modules.PlayerAnchor.Anchor;
using Project.Modules.PlayerAnchor.Anchor.AnchorStates;
using Project.Modules.PlayerAnchor.Chain;
using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField] private InterfaceReference<IPlayerView, MonoBehaviour> _playerView;
        [SerializeField] private HealthBehaviour _playerHealthBehaviour;
        [SerializeField] private PlayerGeneralConfig _playerGeneralConfig;
        [SerializeField] private PlayerMovesetConfig _playerMovesetConfig;
        [SerializeField] private TimeStaminaConfig_SO _playerStaminaConfig;
        
        
        
        [Header("Player States")] 
        [SerializeField] private PlayerStatesConfig _playerStatesConfigurations;

        
        [Space(20)] 
        [Header("ANCHOR")] 
        [SerializeField] private PopeyeAnchor _anchor;
        [SerializeField] private AnchorPhysics _anchorPhysics;
        [SerializeField] private AnchorMotionConfig _anchorMotionConfig;
        [SerializeField] private Transform _anchorMoveTransform;

        [Header("Anchor Damage")] 
        [SerializeField] private AnchorDamageDealer _anchorDamageDealer;
        [SerializeField] private AnchorDamageConfig _anchorDamageConfig;
        
        [Header("Anchor Throwing")]
        [SerializeField] private AnchorThrowConfig _anchorThrowConfig;
        [SerializeField] private AnchorPullConfig _anchorPullConfig;
        [SerializeField] private AnchorTrajectoryEndSpot _anchorTrajectoryEndSpot;
        [SerializeField] private LayerMask _obstaclesMask;
        [SerializeField] private LayerMask _anchorSnapTargetMask;
        
        [Header("CHAIN")]
        [SerializeField] private ChainConfig _anchorChainConfig;
        [SerializeField] private AnchorChain _anchorChain;
        [SerializeField] private InterfaceReference<IChainPhysics, MonoBehaviour> _chainPhysics;

        [SerializeField] private Transform _chainPlayerBindTransform;
        [SerializeField] private Transform _chainAnchorBindTransform;


        [Header("HUD")] 
        [SerializeField] private PlayerHUD _playerHUD;

        
        [Header("DEBUG")]
        [SerializeField] private LineRenderer debugLine;
        [SerializeField] private LineRenderer debugLine2;
        [SerializeField] private LineRenderer debugLine3;
        [SerializeField] private bool drawDebugLines = true;
        private AnchorTrajectoryMaker trajectoryMaker;
        private AnchorThrower AanchorThrower;

        
        
        private void OnValidate()
        {
            if (trajectoryMaker != null)
            {
                trajectoryMaker.drawDebugLines = drawDebugLines;
            }
        }


        private void Awake()
        {
            Install();
            OnValidate();
        }

        private void Install()
        {
            // Services
            ServiceLocator.Instance.RegisterService<ICombatManager>(new CombatManagerService());

            ICombatManager combatManager = ServiceLocator.Instance.GetService<ICombatManager>();

            // Anchor
            TransformMotion anchorMotion = new TransformMotion();
            AnchorThrower anchorThrower = new AnchorThrower();
            AnchorPuller anchorPuller = new AnchorPuller();
            AnchorTrajectoryMaker anchorTrajectoryMaker = new AnchorTrajectoryMaker();
            AnchorStatesBlackboard anchorStatesBlackboard = new AnchorStatesBlackboard();
            AnchorFSM anchorStateMachine = new AnchorFSM();
            IChainPhysics chainPhysics = _chainPhysics.Value;
            AnchorAutoAimController anchorAutoAimController = new AnchorAutoAimController();
            
            
            anchorMotion.Configure(_anchorMoveTransform);
            anchorThrower.Configure(_player, _anchor, anchorTrajectoryMaker, anchorMotion, _anchorThrowConfig, 
                anchorAutoAimController);
            anchorPuller.Configure(_player, _anchor, anchorTrajectoryMaker, anchorMotion, _anchorPullConfig);
            anchorTrajectoryMaker.Configure(_anchorTrajectoryEndSpot, _anchorThrowConfig, _anchorPullConfig,
                debugLine, debugLine2, debugLine3);
            anchorStatesBlackboard.Configure(anchorMotion, _anchorMotionConfig, _anchorPhysics, _anchorChain, 
                _player.AnchorCarryHolder, _player.AnchorGrabToThrowHolder);
            anchorStateMachine.Setup(anchorStatesBlackboard);
            chainPhysics.Configure(_anchorChainConfig);
            anchorAutoAimController.Configure();

            _anchor.Configure(anchorStateMachine, anchorTrajectoryMaker, anchorThrower, anchorPuller, anchorMotion,
                _anchorDamageDealer, _anchorChain);
            _anchorDamageDealer.Configure(_anchorDamageConfig, combatManager, _playerController.LookTransform);
            _anchorPhysics.Configure(_anchor);
            _anchorChain.Configure(chainPhysics, _chainPlayerBindTransform, _chainAnchorBindTransform);

            
            
            // Player
            IMovementInputHandler movementInputHandler = new CameraAxisMovementInput(_isometricCamera.CameraTransform);
            PlayerAnchorMovesetInputsController movesetInputsController = new PlayerAnchorMovesetInputsController();
            PlayerStatesBlackboard playerStatesBlackboard = new PlayerStatesBlackboard();
            TransformMotion playerMotion = new TransformMotion();
            PlayerFSM playerStateMachine = new PlayerFSM();
            TimeStaminaSystem playerStamina = new TimeStaminaSystem(_playerStaminaConfig);
            PlayerHealth playerHealth = new PlayerHealth();
            
            playerStatesBlackboard.Configure(_playerStatesConfigurations, _player, _playerView.Value, 
                movesetInputsController, _anchor);
            playerMotion.Configure(_playerController.Transform, _playerController.Transform);
            playerHealth.Configure(_player, _playerHealthBehaviour, _playerGeneralConfig.MaxHealth,
                _playerGeneralConfig.PotionHealAmount);

            _player.Configure(playerStateMachine, _playerController, _playerMovesetConfig, _playerView.Value, playerHealth, 
                playerStamina, playerMotion, _anchor, anchorThrower, anchorPuller);
            _playerController.MovementInputHandler = movementInputHandler;
            
            playerStateMachine.Setup(playerStatesBlackboard);
            
            // HUD
            _playerHUD.Configure(_playerHealthBehaviour.HealthSystem, playerStamina);
            
            
            // Debug
            trajectoryMaker = anchorTrajectoryMaker;
            AanchorThrower = anchorThrower;
        }


    }
}