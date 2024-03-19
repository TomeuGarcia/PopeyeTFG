using AYellowpaper;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
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
using Popeye.Modules.PlayerAnchor.Player.PlayerEvents;
using Popeye.Modules.PlayerAnchor.Player.PlayerFocus;
using Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts;
using Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops;
using Popeye.Modules.PlayerAnchor.Player.Stamina;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking.OnVoid;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking.OnVoid.VoidPhysics;
using Popeye.Modules.PlayerController.AutoAim;
using Popeye.Modules.VFX.ParticleFactories;
using Popeye.Scripts.Collisions;
using Popeye.Scripts.MaterialHelpers;
using Popeye.Scripts.ObjectTypes;
using Project.Scripts.Time.TimeFunctionalities;
using Project.Scripts.Time.TimeHitStop;
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
        [SerializeField] private HealthBehaviour _playerHealthBehaviour;
        [SerializeField] private PlayerGeneralConfig _playerGeneralConfig;
        [SerializeField] private ObstacleProbingConfig _obstacleProbingConfig;
        [SerializeField] private CollisionProbingConfig _dashFloorProbingConfig;
        [SerializeField] private PlayerAudioFMODConfig _playerAudioConfig;
        [SerializeField] private RenderersMaterialAssigner _playerRenderersMaterialAssigner;

        [Header("Player - Powers")] 
        [SerializeField] private PowerBoostDropFactoryConfig _powerBoostDropFactoryConfig;
        

        [Header("Player - AutoAim")] 
        [SerializeField] private AutoAimCreator _autoAimCreator;

        [Space(20)] 
        [Header("ANCHOR")] 
        [SerializeField] private PopeyeAnchor _anchor;
        [SerializeField] private AnchorPhysics _anchorPhysics;
        [SerializeField] private AnchorCollisions _anchorCollisions;
        [SerializeField] private VFXAnchorView _vfxAnchorView;
        [SerializeField] private DropShadowBehaviour _anchorDropShadow;
        [SerializeField] private AnchorGeneralConfig _anchorGeneralConfig;
        [SerializeField] private AnchorAudioFMODConfig _anchorAudioConfig;
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


        public IPlayerMediator PlayerMediator => _player;
        

        public void Install()
        {
            // Services
            ServiceLocator.Instance.RegisterService<ICameraFunctionalities>(new CameraFunctionalities(
                new CameraZoomer(_isometricCamera.Value), _cameraShaker.Value));
            
            
            ICameraFunctionalities cameraFunctionalities = ServiceLocator.Instance.GetService<ICameraFunctionalities>();
            ICombatManager combatManager = ServiceLocator.Instance.GetService<ICombatManager>();
            IFMODAudioManager fmodAudioManager = ServiceLocator.Instance.GetService<IFMODAudioManager>();
            IParticleFactory particleFactory = ServiceLocator.Instance.GetService<IParticleFactory>();
            ITimeFunctionalities timeFunctionalities = ServiceLocator.Instance.GetService<ITimeFunctionalities>();
            IEventSystemService eventSystemService = ServiceLocator.Instance.GetService<IEventSystemService>();

            
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
            IOnVoidChecker anchorOnVoidChecker = CreateOnVoidChecker(_anchor.PositionTransform, _anchorGeneralConfig.OnVoidProbingConfig);
            IAnchorTrajectoryView anchorTrajectoryView = new BezierAnchorTrajectoryView(
                _anchorTrajectoryLine1, _anchorTrajectoryLine2, 
                _anchorGeneralConfig.TrajectoryConfig.ViewConfig, _anchorGeneralConfig.TrajectoryConfig.NumberOfPoints);
            IAnchorViewExtras anchorViewExtras = new AnchorViewExtras(_anchorDropShadow);
            
            Material chainMaterialCopy = new Material(chainViewLogicGeneralConfig.BoneSharedMaterial);
            chainViewLogicGeneralConfig.ApplyMaterialToBonePrefabs(chainMaterialCopy);
            IVFXChainView vfxChainView = new GhostVFXChainView(chainViewLogicGeneralConfig.ObstacleCollisionProbingConfig, chainMaterialCopy, 
                _player.AnchorGrabToThrowHolder);

            IAnchorAudio anchorAudio = new AnchorAudioFMOD(_anchor.PositionTransform.gameObject, fmodAudioManager, _anchorAudioConfig);
            IThrowDistanceComputer throwDistanceComputer =
                new MovingForwardRangeThrowDistanceComputer(_anchorGeneralConfig.ThrowConfig, _playerController);

            IAnchorView anchorView = CreateAnchorView(_anchorGeneralConfig.GeneralViewConfig, _anchor.MeshHolder,
                particleFactory, timeFunctionalities.HitStopManager, cameraFunctionalities.CameraShaker);
            
            anchorMotion.Configure(_anchor.PositionTransform);
            anchorThrower.Configure(_player, _anchor, anchorTrajectoryMaker, throwDistanceComputer,
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

            _anchorDamageDealer.Configure(_anchor, _anchorGeneralConfig.DamageConfig, combatManager, 
                _playerController.LookTransform);
            _anchorPhysics.Configure(_anchor);
            _anchorChain.Configure(chainPhysics, vfxChainView, _chainPlayerBindTransform, _chainAnchorBindTransform, 
                chainViewLogicGeneralConfig);
            _anchor.Configure(anchorStateMachine, anchorTrajectoryMaker, anchorThrower, anchorPuller, anchorMotion,
                _anchorPhysics, _anchorCollisions, anchorView, anchorViewExtras, anchorAudio, 
                _anchorDamageDealer, _anchorChain, cameraFunctionalities, anchorOnVoidChecker);

            IAnchorStatesCreator anchorStatesCreator = _generalGameStateData.IsTutorial
                ? new TutorialAnchorStatesCreator()
                : new DefaultAnchorStatesCreator();
            anchorStateMachine.Configure(anchorStatesBlackboard, anchorStatesCreator);

                
            
            // Player
            IMovementInputHandler movementInputHandler = new CameraAxisMovementInput(_isometricCamera.Value.CameraTransform);
            PlayerAnchorMovesetInputsController movesetInputsController = new PlayerAnchorMovesetInputsController(eventSystemService);
            PlayerStatesBlackboard playerStatesBlackboard = new PlayerStatesBlackboard();
            TransformMotion playerMotion = new TransformMotion();
            PlayerFSM playerStateMachine = new PlayerFSM();
            PlayerStaminaSystem playerStamina = new PlayerStaminaSystem(_playerGeneralConfig.StaminaConfig);
            PlayerHealth playerHealth = new PlayerHealth();
            PlayerDasher playerDasher = new PlayerDasher();
            PlayerMovementChecker playerMovementChecker = new PlayerMovementChecker();
            ISafeGroundChecker playerSafeGroundChecker = CreateSafeGroundChecker(_playerController.Transform, 
                _playerGeneralConfig.SafeGroundProbingConfig, _playerGeneralConfig.NotSafeGroundType);
            IOnVoidChecker playerOnVoidChecker = CreateOnVoidChecker(_playerController.Transform, _playerGeneralConfig.OnVoidProbingConfig);

            Material playerMaterial = _playerRenderersMaterialAssigner.AssignToRenderersAndGetMaterial();
            IPlayerView playerView = CreatePlayerView(_playerGeneralConfig.GeneralViewConfig, _player, playerMaterial);
            IPlayerAudio playerAudio = new PlayerAudioFMOD(_playerController.gameObject, fmodAudioManager, _playerAudioConfig);

            PlayerFocusController playerFocusController =
                new PlayerFocusController(_playerGeneralConfig.FocusConfig, _playerHUD.PlayerFocusUI);
            ISpecialAttackToggleable[] specialAttackToggleables = 
                { _anchorGeneralConfig.DamageConfig, _playerGeneralConfig.StatesConfig };
            PlayerFocusSpecialAttackController playerSpecialAttackController
                = new PlayerFocusSpecialAttackController(playerFocusController, _playerGeneralConfig.FocusConfig.AttackConfig,
                    specialAttackToggleables);
            
            IPlayerHealing playerHealing = 
                new FocusPlayerHealing(playerHealth, _playerGeneralConfig.FocusConfig.HealingConfig, playerFocusController);

            PlayerGlobalEventsListener playerGlobalEventsListener = 
                new PlayerGlobalEventsListener(eventSystemService, _player, _anchor);
            PlayerEventsDispatcher playerEventsDispatcher =
                new PlayerEventsDispatcher(eventSystemService);
            
            _playerController.AwakeConfigure();
            playerStatesBlackboard.Configure(_playerGeneralConfig.StatesConfig, _player, playerView, 
                movesetInputsController, _anchor, playerMovementChecker);
            playerMotion.Configure(_playerController.Transform, _playerController.Transform);
            playerHealth.Configure(_player, _playerHealthBehaviour, _playerGeneralConfig.PlayerHealthConfig.MaxHealth,
                _playerController.Rigidbody, _playerGeneralConfig.VoidFallDamageConfig);
            playerDasher.Configure(_player, _anchor, _playerGeneralConfig, playerMotion, 
                _obstacleProbingConfig, _dashFloorProbingConfig);
            playerMovementChecker.Configure(_player, _playerController);


            _playerController.MovementInputHandler = movementInputHandler;
            _playerController.InputCorrector =
                new AutoAimInputCorrector(_autoAimCreator.Create(_playerController.LookTransform));
            
            _player.Configure(playerStateMachine, _playerController, _playerGeneralConfig, _anchorGeneralConfig, 
                playerView, playerAudio, playerHealing, playerHealth, playerStamina, playerMovementChecker, playerMotion, playerDasher,
                _anchor, anchorThrower, anchorPuller, anchorKicker, anchorSpinner,
                playerSafeGroundChecker, playerOnVoidChecker, playerFocusController, playerSpecialAttackController,
                playerGlobalEventsListener, playerEventsDispatcher);


            IPlayerStatesCreator playerStatesCreator = _generalGameStateData.IsTutorial
                ? new TutorialPlayerStatesCreator()
                : new DefaultPlayerStatesCreator();
            playerStateMachine.Configure(playerStatesBlackboard, playerStatesCreator);
            
            // HUD
            _playerHUD.Configure(_playerHealthBehaviour.HealthSystem, playerStamina.BaseStamina, playerFocusController);
            
            
            PowerBoostDropFactory powerBoostDropFactory =
                new PowerBoostDropFactory(_powerBoostDropFactoryConfig, transform, _playerController.Transform);
            
            
            ServiceLocator.Instance.RegisterService<IPowerBoostDropFactory>(powerBoostDropFactory);
            
        }


        public void Uninstall()
        {
            ServiceLocator.Instance.RemoveService<IPowerBoostDropFactory>();
            ServiceLocator.Instance.RemoveService<ICameraFunctionalities>();
        }



        private IPlayerView CreatePlayerView(PlayerGeneralViewConfig playerGeneralViewConfig, PopeyePlayer player, Material playerMaterial)
        {
            IPlayerView playerSquashAndStretchView = new PlayerSquashAndStretchView(
                playerGeneralViewConfig.SquashStretchViewConfig,
                player.MeshHolderTransform
            );

            IPlayerView playerMaterialView = new PlayerMaterialView(
                playerGeneralViewConfig.MaterialViewConfig,
                playerMaterial,
                player.Renderer.transform
            );

            IPlayerView playerParticleView = new PlayerParticlesView(
                playerGeneralViewConfig.ParticlesViewConfig,
                player.MeshHolderTransform,
                ServiceLocator.Instance.GetService<IParticleFactory>()
            );
            
            IPlayerView playerGameFeelEffectsView = new PlayerGameFeelEffectsView(
                playerGeneralViewConfig.GameFeelEffectsViewConfig,
                ServiceLocator.Instance.GetService<ITimeFunctionalities>().HitStopManager,
                ServiceLocator.Instance.GetService<ICameraFunctionalities>().CameraShaker,
                ServiceLocator.Instance.GetService<ICameraFunctionalities>().CameraZoomer
            );

            IPlayerView animationsPlayerView = new PlayerAnimationsView(
                _playerGeneralConfig.GeneralViewConfig.AnimatorViewConfig,
                _player.Animator
            );


            IPlayerView[] playerSubViews =
            {
                playerSquashAndStretchView,
                playerMaterialView,
                playerParticleView,
                playerGameFeelEffectsView,
                animationsPlayerView
            };
            
            
            PlayerGeneralView playerGeneralView = new PlayerGeneralView(playerSubViews);

            return playerGeneralView;
        }
        
        private IAnchorView CreateAnchorView(GeneralAnchorViewConfig anchorGeneralViewConfig, Transform anchorMeshHolder,
            IParticleFactory particleFactory, IHitStopManager hitStopManager, ICameraShaker cameraShaker)
        {
            IAnchorView anchorStretchView = new StretchAnchorView(
                anchorGeneralViewConfig.StretchViewConfig, anchorMeshHolder
            );
            
            _vfxAnchorView.Configure(
                anchorGeneralViewConfig.VfxViewConfig,
                particleFactory,
                hitStopManager,
                cameraShaker
            );

            IAnchorView[] anchorSubViews =
            {
                anchorStretchView,
                _vfxAnchorView
            };
            
            
            GeneralAnchorView anchorGeneralView = new GeneralAnchorView(anchorSubViews);

            return anchorGeneralView;
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