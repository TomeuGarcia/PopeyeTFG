using System;
using AYellowpaper;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.PlayerAnchor.Player;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using Popeye.Modules.PlayerController;
using Popeye.Modules.PlayerController.Inputs;
using Popeye.Modules.Camera;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.Camera.CameraZoom;
using Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations;
using Popeye.Modules.ValueStatSystem;
using Project.Modules.CombatSystem;
using Project.Modules.PlayerAnchor.Anchor;
using Project.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
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
        [SerializeField] private InterfaceReference<ICameraShaker, MonoBehaviour> _cameraShaker;
        
        
        [Space(20)]
        [Header("PLAYER")]
        [SerializeField] private PopeyePlayer _player;
        [SerializeField] private Popeye.Modules.PlayerController.PlayerController _playerController;
        [SerializeField] private InterfaceReference<IPlayerView, MonoBehaviour> _playerView;
        [SerializeField] private HealthBehaviour _playerHealthBehaviour;
        [SerializeField] private PlayerGeneralConfig _playerGeneralConfig;
        [SerializeField] private ObstacleProbingConfig _obstacleProbingConfig;
       
        
        
        [Space(20)] 
        [Header("ANCHOR")] 
        [SerializeField] private PopeyeAnchor _anchor;
        [SerializeField] private AnchorPhysics _anchorPhysics;
        [SerializeField] private InterfaceReference<IAnchorView, MonoBehaviour> _anchorView;
        [SerializeField] private AnchorGeneralConfig _anchorGeneralConfig;
        [SerializeField] private Transform _anchorMoveTransform;

        
        [Header("Anchor Damage")] 
        [SerializeField] private AnchorDamageDealer _anchorDamageDealer;
        
        [Header("Anchor Throwing")]
        [SerializeField] private AnchorTrajectoryEndSpot _anchorTrajectoryEndSpot;

        [Header("CHAIN")]
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


        private void Start()
        {
            OnValidate();
        }

        public void Install()
        {
            // Services
            ServiceLocator.Instance.RegisterService<ICameraFunctionalities>(new CameraFunctionalities(
                new CameraZoomer(_isometricCamera), _cameraShaker.Value));
            
            ServiceLocator.Instance.RegisterService<IGameReferences>(new GameReferences(_player.GetTargetForEnemies()));
            
            
            ICameraFunctionalities cameraFunctionalities = ServiceLocator.Instance.GetService<ICameraFunctionalities>();
            ICombatManager combatManager = ServiceLocator.Instance.GetService<ICombatManager>();

            
            // Anchor
            TransformMotion anchorMotion = new TransformMotion();
            AnchorThrower anchorThrower = new AnchorThrower();
            AnchorPuller anchorPuller = new AnchorPuller();
            AnchorKicker anchorKicker = new AnchorKicker();
            AnchorSpinner anchorSpinner = new AnchorSpinner();
            AnchorTrajectoryMaker anchorTrajectoryMaker = new AnchorTrajectoryMaker();
            AnchorStatesBlackboard anchorStatesBlackboard = new AnchorStatesBlackboard();
            AnchorFSM anchorStateMachine = new AnchorFSM();
            IChainPhysics chainPhysics = _chainPhysics.Value;
            AnchorAutoAimController anchorAutoAimController = new AnchorAutoAimController();
            
            
            anchorMotion.Configure(_anchorMoveTransform);
            anchorThrower.Configure(_player, _anchor, anchorTrajectoryMaker,  
                _anchorGeneralConfig.ThrowConfig, _anchorGeneralConfig.VerticalThrowConfig, 
                anchorAutoAimController);
            anchorPuller.Configure(_player, _anchor, anchorTrajectoryMaker, _anchorGeneralConfig.PullConfig);
            anchorKicker.Configure(_player, _anchor, anchorTrajectoryMaker, _anchorGeneralConfig.KickConfig);
            anchorSpinner.Configure(_player, _anchor, _anchorGeneralConfig.SpinConfig);
            anchorTrajectoryMaker.Configure(_anchorTrajectoryEndSpot, _obstacleProbingConfig, 
                _anchorGeneralConfig.PullConfig, debugLine, debugLine2, debugLine3);
            anchorStatesBlackboard.Configure(anchorMotion, _anchorGeneralConfig.MotionConfig, _anchorPhysics, _anchorChain, 
                _player.AnchorCarryHolder, _player.AnchorGrabToThrowHolder);
            anchorStateMachine.Setup(anchorStatesBlackboard);
            chainPhysics.Configure(_anchorGeneralConfig.ChainConfig);
            anchorAutoAimController.Configure();

            _anchorDamageDealer.Configure(_anchorGeneralConfig.DamageConfig, combatManager, _playerController.LookTransform);
            _anchorPhysics.Configure(_anchor);
            _anchorChain.Configure(chainPhysics, _chainPlayerBindTransform, _chainAnchorBindTransform);
            _anchor.Configure(anchorStateMachine, anchorTrajectoryMaker, anchorThrower, anchorPuller, anchorMotion,
                _anchorPhysics, _anchorView.Value, _anchorDamageDealer, _anchorChain, cameraFunctionalities);

            
            
            // Player
            IMovementInputHandler movementInputHandler = new CameraAxisMovementInput(_isometricCamera.CameraTransform);
            PlayerAnchorMovesetInputsController movesetInputsController = new PlayerAnchorMovesetInputsController();
            PlayerStatesBlackboard playerStatesBlackboard = new PlayerStatesBlackboard();
            TransformMotion playerMotion = new TransformMotion();
            PlayerFSM playerStateMachine = new PlayerFSM();
            TimeStaminaSystem playerStamina = new TimeStaminaSystem(_playerGeneralConfig.StaminaConfig);
            PlayerHealth playerHealth = new PlayerHealth();
            PlayerDasher playerDasher = new PlayerDasher();
            
            
            playerStatesBlackboard.Configure(_playerGeneralConfig.StatesConfig, _player, _playerView.Value, 
                movesetInputsController, _anchor);
            playerMotion.Configure(_playerController.Transform, _playerController.Transform);
            playerHealth.Configure(_player, _playerHealthBehaviour, _playerGeneralConfig.MaxHealth,
                _playerGeneralConfig.PotionHealAmount);
            playerDasher.Configure(_player, _anchor, _playerGeneralConfig, playerMotion, _obstacleProbingConfig);

            _player.Configure(playerStateMachine, _playerController, _playerGeneralConfig, _anchorGeneralConfig, 
                _playerView.Value, playerHealth, playerStamina, playerMotion, playerDasher,
                _anchor, anchorThrower, anchorPuller, anchorKicker, anchorSpinner);
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