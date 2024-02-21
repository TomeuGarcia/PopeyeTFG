using AYellowpaper;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.PlayerAnchor.Player;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using Popeye.Modules.PlayerController.Inputs;
using Popeye.Modules.Camera;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.Camera.CameraZoom;
using Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations;
using Popeye.Modules.ValueStatSystem;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.GameState.GaneralGameState;
using Popeye.Modules.PlayerAnchor.Anchor;
using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using Popeye.Modules.PlayerAnchor.Anchor.AnchorStates;
using Popeye.Modules.PlayerAnchor.Chain;
using Popeye.Modules.PlayerAnchor.DropShadow;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking.OnVoid;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking.OnVoid.VoidPhysics;
using Popeye.Modules.PlayerController.AutoAim;
using Popeye.Scripts.Collisions;
using Popeye.Scripts.ObjectTypes;
using UnityEngine;
using UnityEngine.Serialization;


namespace Popeye.Modules.PlayerAnchor
{
    public class PlayerAnchorInstaller : MonoBehaviour
    {
        [Header("GAME STATE")] 
        [SerializeField] private GeneralGameStateData _generalGameStateData;
        
        [Header("CAMERA")] 
        [SerializeField] private InterfaceReference<ICameraController, MonoBehaviour> _isometricCamera;
        [SerializeField] private InterfaceReference<ICameraShaker, MonoBehaviour> _cameraShaker;
        
        
        [Space(20)]
        [Header("PLAYER")]
        [SerializeField] private PopeyePlayer _player;
        [SerializeField] private Popeye.Modules.PlayerController.PlayerController _playerController;
        [SerializeField] private InterfaceReference<IPlayerView, MonoBehaviour> _playerView;
        [SerializeField] private HealthBehaviour _playerHealthBehaviour;
        [SerializeField] private PlayerGeneralConfig _playerGeneralConfig;
        [SerializeField] private ObstacleProbingConfig _obstacleProbingConfig;
        [SerializeField] private CollisionProbingConfig _dashFloorProbingConfig;
        [SerializeField] private InterfaceReference<IPlayerAudio, MonoBehaviour> _playerAudioRef;

        [Header("Player - AutoAim")] 
        [SerializeField] private AutoAimCreator _autoAimCreator;

        [Space(20)] 
        [Header("ANCHOR")] 
        [SerializeField] private PopeyeAnchor _anchor;
        [SerializeField] private AnchorPhysics _anchorPhysics;
        [SerializeField] private AnchorCollisions _anchorCollisions;
        [SerializeField] private InterfaceReference<IAnchorView, MonoBehaviour> _anchorView;
        [SerializeField] private DropShadowBehaviour _anchorDropShadow;
        [SerializeField] private AnchorGeneralConfig _anchorGeneralConfig;
        [SerializeField] private Transform _anchorMoveTransform;
        [SerializeField] private InterfaceReference<IAnchorAudio, MonoBehaviour> _anchorAudioRef;
        [SerializeField] private LineRenderer _anchorTrajectoryLine1;
        [SerializeField] private LineRenderer _anchorTrajectoryLine2;

        
        [Header("Anchor Damage")] 
        [SerializeField] private AnchorDamageDealer _anchorDamageDealer;
        
        [Header("Anchor Throwing")]
        [SerializeField] private AnchorTrajectoryEndSpot _anchorTrajectoryEndSpot;

        [Header("CHAIN")]
        [SerializeField] private AnchorChain _anchorChain;
        [SerializeField] private InterfaceReference<IChainPhysics, MonoBehaviour> _chainPhysics;
        [SerializeField] private ChainViewLogicGeneralConfig chainViewLogicGeneralConfig;

        
        [SerializeField] private Transform _chainPlayerBindTransform;
        [SerializeField] private Transform _chainAnchorBindTransform;


        [Header("HUD")] 
        [SerializeField] private PlayerHUD _playerHUD;



        public void Install()
        {
            // Services
            ServiceLocator.Instance.RegisterService<ICameraFunctionalities>(new CameraFunctionalities(
                new CameraZoomer(_isometricCamera.Value), _cameraShaker.Value));
            
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
            AnchorTrajectorySnapController anchorTrajectorySnapController = new AnchorTrajectorySnapController();
            IAnchorAudio anchorAudio = _anchorAudioRef.Value;
            IOnVoidChecker anchorOnVoidChecker = CreateOnVoidChecker(_anchorMoveTransform, _anchorGeneralConfig.OnVoidProbingConfig);
            IAnchorTrajectoryView anchorTrajectoryView = new BezierAnchorTrajectoryView(
                _anchorTrajectoryLine1, _anchorTrajectoryLine2, 
                _anchorGeneralConfig.TrajectoryConfig.ViewConfig, _anchorGeneralConfig.TrajectoryConfig.NumberOfPoints);
            IAnchorViewExtras anchorViewExtras = new AnchorViewExtras(_anchorDropShadow);
            
            
            
            anchorMotion.Configure(_anchorMoveTransform);
            anchorThrower.Configure(_player, _anchor, anchorTrajectoryMaker,  
                _anchorGeneralConfig.ThrowConfig, _anchorGeneralConfig.VerticalThrowConfig, 
                anchorTrajectorySnapController, anchorTrajectoryView);
            anchorPuller.Configure(_player, _anchor, anchorTrajectoryMaker, _anchorGeneralConfig.PullConfig);
            anchorKicker.Configure(_player, _anchor, anchorTrajectoryMaker, _anchorGeneralConfig.KickConfig);
            anchorSpinner.Configure(_player, _anchor, _anchorGeneralConfig.SpinConfig);
            anchorTrajectoryMaker.Configure(_anchorTrajectoryEndSpot, _obstacleProbingConfig, 
                _anchorGeneralConfig.PullConfig, _anchorGeneralConfig.TrajectoryConfig.NumberOfPoints);
            anchorStatesBlackboard.Configure(_anchor, anchorMotion, _anchorGeneralConfig.MotionConfig, _anchorPhysics, 
                _anchorChain, _player.AnchorCarryHolder, _player.AnchorGrabToThrowHolder, _playerController.Transform);
            chainPhysics.Configure(_anchorGeneralConfig.ChainConfig);
            anchorTrajectorySnapController.Configure();
            _anchorCollisions.Configure(_obstacleProbingConfig);
            anchorAudio.Configure(_anchorMoveTransform.gameObject);

            _anchorDamageDealer.Configure(_anchor, _anchorGeneralConfig.DamageConfig, combatManager, 
                _playerController.LookTransform);
            _anchorPhysics.Configure(_anchor);
            _anchorChain.Configure(chainPhysics, _chainPlayerBindTransform, _chainAnchorBindTransform, chainViewLogicGeneralConfig);
            _anchor.Configure(anchorStateMachine, anchorTrajectoryMaker, anchorThrower, anchorPuller, anchorMotion,
                _anchorPhysics, _anchorCollisions, _anchorView.Value, anchorViewExtras, anchorAudio, 
                _anchorDamageDealer, _anchorChain, cameraFunctionalities, anchorOnVoidChecker);

            IAnchorStatesCreator anchorStatesCreator = _generalGameStateData.IsTutorial
                ? new TutorialAnchorStatesCreator()
                : new DefaultAnchorStatesCreator();
            anchorStateMachine.Configure(anchorStatesBlackboard, anchorStatesCreator);

                
            
            // Player
            IMovementInputHandler movementInputHandler = new CameraAxisMovementInput(_isometricCamera.Value.CameraTransform);
            PlayerAnchorMovesetInputsController movesetInputsController = new PlayerAnchorMovesetInputsController();
            PlayerStatesBlackboard playerStatesBlackboard = new PlayerStatesBlackboard();
            TransformMotion playerMotion = new TransformMotion();
            PlayerFSM playerStateMachine = new PlayerFSM();
            TimeStaminaSystem playerStamina = new TimeStaminaSystem(_playerGeneralConfig.StaminaConfig);
            PlayerHealth playerHealth = new PlayerHealth();
            PlayerDasher playerDasher = new PlayerDasher();
            PlayerMovementChecker playerMovementChecker = new PlayerMovementChecker();
            IPlayerAudio playerAudio = _playerAudioRef.Value;
            ISafeGroundChecker playerSafeGroundChecker = CreateSafeGroundChecker(_playerController.Transform, 
                _playerGeneralConfig.SafeGroundProbingConfig, _playerGeneralConfig.NotSafeGroundType);
            IOnVoidChecker playerOnVoidChecker = CreateOnVoidChecker(_playerController.Transform, _playerGeneralConfig.OnVoidProbingConfig);
            
            
            _playerController.AwakeConfigure();
            playerStatesBlackboard.Configure(_playerGeneralConfig.StatesConfig, _player, _playerView.Value, 
                movesetInputsController, _anchor);
            playerMotion.Configure(_playerController.Transform, _playerController.Transform);
            playerHealth.Configure(_player, _playerHealthBehaviour, _playerGeneralConfig.MaxHealth,
                _playerGeneralConfig.PotionHealAmount, _playerController.Rigidbody, _playerGeneralConfig.VoidFallDamageConfig);
            playerDasher.Configure(_player, _anchor, _playerGeneralConfig, playerMotion, 
                _obstacleProbingConfig, _dashFloorProbingConfig);
            playerMovementChecker.Configure(_player, _playerController);
            playerAudio.Configure(_playerController.gameObject);


            _playerController.MovementInputHandler = movementInputHandler;
            _playerController.InputCorrector =
                new AutoAimInputCorrector(_autoAimCreator.Create(_playerController.LookTransform));
            
            _player.Configure(playerStateMachine, _playerController, _playerGeneralConfig, _anchorGeneralConfig, 
                _playerView.Value, playerAudio, playerHealth, playerStamina, playerMovementChecker, playerMotion, playerDasher,
                _anchor, anchorThrower, anchorPuller, anchorKicker, anchorSpinner,
                playerSafeGroundChecker, playerOnVoidChecker);


            IPlayerStatesCreator playerStatesCreator = _generalGameStateData.IsTutorial
                ? new TutorialPlayerStatesCreator()
                : new DefaultPlayerStatesCreator();
            playerStateMachine.Configure(playerStatesBlackboard, playerStatesCreator);
            
            // HUD
            _playerHUD.Configure(_playerHealthBehaviour.HealthSystem, playerStamina);
            
        }


        public void Uninstall()
        {
            ServiceLocator.Instance.RemoveService<ICameraFunctionalities>();
            ServiceLocator.Instance.RemoveService<IGameReferences>();
        }



        private IOnVoidChecker CreateOnVoidChecker(Transform castOriginTransform, CollisionProbingConfig voidProbingConfig,
            float checkFrequency = 0.15f)
        {
            ICastComputer castComputer = new CastComputerGlobal(castOriginTransform, Vector3.up * 1, Vector3.down);
            PhysicsCastRequirementsProcessor castRequirementsProcessor = new PhysicsCastRequirementsProcessor();
            
            IPhysicsCaster physicsCaster = 
                new PhysicsSphereCaster(castComputer, voidProbingConfig, castRequirementsProcessor, 0.5f);

            IOnVoidChecker onVoidChecker = new OnVoidPhysicsChecker(physicsCaster, checkFrequency); 
            
            return onVoidChecker;
        }
        
        private ISafeGroundChecker CreateSafeGroundChecker(Transform trackingTransform, 
            CollisionProbingConfig groundProbingConfig, ObjectTypeAsset safeGroundIgnoreType,
            float checkFrequency = 1.0f)
        {
            ICastComputer castComputer = new CastComputerGlobal(trackingTransform, Vector3.up * 2, Vector3.down);
            
            IPhysicsCastRequirement[] physicsCastRequirements =
            {
                new IgnoreObjectTypeRequirement(safeGroundIgnoreType)
            };
            PhysicsCastRequirementsProcessor castRequirementsProcessor =
                new PhysicsCastRequirementsProcessor(physicsCastRequirements);

            IPhysicsCaster physicsCaster =
                new PhysicsRayCaster(castComputer, groundProbingConfig, castRequirementsProcessor);
            
            
            return new SafeGroundPhysicsChecker(trackingTransform, physicsCaster, checkFrequency, 1.0f);
        }
    }
}