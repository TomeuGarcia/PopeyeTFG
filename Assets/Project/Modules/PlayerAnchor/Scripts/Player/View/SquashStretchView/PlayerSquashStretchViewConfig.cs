using NaughtyAttributes;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    
    [System.Serializable]
    public class PlayerSquashStretchViewConfig
    {
        [Header("TAKE DAMAGE")]
        [Expandable] [SerializeField] private TweenPunchConfig _takeDamageScalePunch;
        public TweenPunchConfig TakeDamageScalePunch => _takeDamageScalePunch;
      
        
        [Header("HEAL")]
        [Expandable] [SerializeField] private TweenPunchConfig _healScalePunch;
        [Expandable] [SerializeField] private TweenPunchConfig _startHealingScalePunch;
        public TweenPunchConfig HealScalePunch => _healScalePunch;
        public TweenPunchConfig StartHealingScalePunch => _startHealingScalePunch;
        

        [Header("DEATH")] 
        [Expandable] [SerializeField] private TweenConfig _deathRotation;
        [Expandable] [SerializeField] private TweenConfig _deathMoveBy;
        public TweenConfig DeathRotation => _deathRotation;
        public TweenConfig DeathMoveBy => _deathMoveBy;


        [Header("DASH")]
        [SerializeField] private TweenPunchConfig _dashScalePunch;
        [SerializeField] private TweenPunchConfig _dashRotationPunch;
        public TweenPunchConfig DashScalePunch => _dashScalePunch;
        public TweenPunchConfig DashRotationPunch => _dashRotationPunch;
        
        
        [Header("KICK")]
        [SerializeField] private TweenPunchConfig _kickScalePunch;
        [SerializeField] private TweenPunchConfig _kickRotationPunch;
        public TweenPunchConfig KickScalePunch => _kickScalePunch;
        public TweenPunchConfig KickRotationPunch => _kickRotationPunch;
        
        
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