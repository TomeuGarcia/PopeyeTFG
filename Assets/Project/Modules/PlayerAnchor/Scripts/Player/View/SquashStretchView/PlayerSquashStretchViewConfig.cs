using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    
    [System.Serializable]
    public class PlayerSquashStretchViewConfig
    {
        [Header("TAKE DAMAGE")]
        [SerializeField] private PlayerSquashAndStretchView.TweenPunchData _takeDamageScalePunch;
        public PlayerSquashAndStretchView.TweenPunchData TakeDamageScalePunch => _takeDamageScalePunch;
      
        
        [Header("HEAL")]
        [SerializeField] private PlayerSquashAndStretchView.TweenPunchData _healScalePunch;
        public PlayerSquashAndStretchView.TweenPunchData HealScalePunch => _healScalePunch;


        [Header("DEATH")] 
        [SerializeField] private PlayerSquashAndStretchView.TweenData _deathRotation;
        [SerializeField] private PlayerSquashAndStretchView.TweenData _deathMoveBy;
        public PlayerSquashAndStretchView.TweenData DeathRotation => _deathRotation;
        public PlayerSquashAndStretchView.TweenData DeathMoveBy => _deathMoveBy;


        [Header("DASH")]
        [SerializeField] private PlayerSquashAndStretchView.TweenPunchData _dashScalePunch;
        [SerializeField] private PlayerSquashAndStretchView.TweenPunchData _dashRotationPunch;
        public PlayerSquashAndStretchView.TweenPunchData DashScalePunch => _dashScalePunch;
        public PlayerSquashAndStretchView.TweenPunchData DashRotationPunch => _dashRotationPunch;
        
        
        [Header("KICK")]
        [SerializeField] private PlayerSquashAndStretchView.TweenPunchData _kickScalePunch;
        [SerializeField] private PlayerSquashAndStretchView.TweenPunchData _kickRotationPunch;
        public PlayerSquashAndStretchView.TweenPunchData KickScalePunch => _kickScalePunch;
        public PlayerSquashAndStretchView.TweenPunchData KickRotationPunch => _kickRotationPunch;
        
        
        [Header("THROW")]
        [SerializeField] private PlayerSquashAndStretchView.TweenPunchData _throwScalePunch;
        [SerializeField] private PlayerSquashAndStretchView.TweenPunchData _throwRotationPunch;
        public PlayerSquashAndStretchView.TweenPunchData ThrowScalePunch => _throwScalePunch;
        public PlayerSquashAndStretchView.TweenPunchData ThrowRotationPunch => _throwRotationPunch;
        
        
        [Header("PULL")]
        [SerializeField] private PlayerSquashAndStretchView.TweenPunchData _pullScalePunch;
        [SerializeField] private PlayerSquashAndStretchView.TweenPunchData _pullRotationPunch;
        public PlayerSquashAndStretchView.TweenPunchData PullScalePunch => _pullScalePunch;
        public PlayerSquashAndStretchView.TweenPunchData PullRotationPunch => _pullRotationPunch;
        
        
        [Header("ANCHOR OBSTRUCTED")]
        [SerializeField] private PlayerSquashAndStretchView.TweenPunchData _anchorObstructedRotationPunch;
        public PlayerSquashAndStretchView.TweenPunchData AnchorObstructedRotationPunch => _anchorObstructedRotationPunch;
        
        
    }
}