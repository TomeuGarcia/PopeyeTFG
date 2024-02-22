using Cysharp.Threading.Tasks;
using Popeye.Modules.Camera.CameraShake;
using Project.Scripts.Time.TimeHitStop;

namespace Popeye.Modules.PlayerAnchor.Player.GameFeelEffects
{
    public class PlayerGameFeelEffectsView : IPlayerView
    {
        private readonly PlayerGameFeelEffectsConfig _config;
        private readonly IHitStopManager _hitStopManager;
        private readonly ICameraShaker _cameraShaker;

        
        
        public PlayerGameFeelEffectsView(PlayerGameFeelEffectsConfig config,
            IHitStopManager hitStopManager, ICameraShaker cameraShaker)
        {
            _config = config;
            _hitStopManager = hitStopManager;
            _cameraShaker = cameraShaker;
        }
        
        
        public void StartTired()
        {
        }

        public void EndTired()
        {
        }

        public void PlayTakeDamageAnimation()
        {
            _hitStopManager.QueueHitStop(_config.TakeDamageHitStop);
            _cameraShaker.PlayShake(_config.TakeDamageCameraShake);
        }

        public void PlayRespawnAnimation()
        {
        }

        public void PlayDeathAnimation()
        {
        }

        public void PlayHealAnimation()
        {
        }

        public void PlayDashAnimation(float duration)
        {
        }

        public void PlayKickAnimation()
        {
        }

        public void PlayThrowAnimation()
        {
        }

        public async UniTaskVoid PlayPullAnimation(float delay)
        {
            
        }

        public void PlayAnchorObstructedAnimation()
        {
        }
    }
}