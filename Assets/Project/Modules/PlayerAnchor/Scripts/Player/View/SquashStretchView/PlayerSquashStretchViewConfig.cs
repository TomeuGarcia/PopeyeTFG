using NaughtyAttributes;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    
    [System.Serializable]
    public class PlayerSquashStretchViewConfig
    {
        [Header("TAKE DAMAGE")]
        [SerializeField] private TweenPunchConfig _takeDamageScalePunch;
        public TweenPunchConfig TakeDamageScalePunch => _takeDamageScalePunch;
      
        
        [Header("HEAL")]
        [SerializeField] private TweenPunchConfig _healScalePunch;
        [SerializeField] private TweenPunchConfig _startHealingScalePunch;
        public TweenPunchConfig HealScalePunch => _healScalePunch;
        public TweenPunchConfig StartHealingScalePunch => _startHealingScalePunch;
        

        [Header("DEATH")]
        [SerializeField] private TweenConfig _deathRotation;
        [SerializeField] private TweenConfig _deathMoveBy;
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
        [SerializeField] private TweenPunchConfig _throwScalePunch;
        [SerializeField] private TweenPunchConfig _throwRotationPunch;
        public TweenPunchConfig ThrowScalePunch => _throwScalePunch;
        public TweenPunchConfig ThrowRotationPunch => _throwRotationPunch;
        
        
        [Header("PULL")]
        [SerializeField] private TweenPunchConfig _pullScalePunch;
        [SerializeField] private TweenPunchConfig _pullRotationPunch;
        public TweenPunchConfig PullScalePunch => _pullScalePunch;
        public TweenPunchConfig PullRotationPunch => _pullRotationPunch;
        
        
        [Header("ANCHOR OBSTRUCTED")]
        [SerializeField] private TweenPunchConfig _anchorObstructedRotationPunch;
        public TweenPunchConfig AnchorObstructedRotationPunch => _anchorObstructedRotationPunch;
                
        
        [Header("SPECIAL ATTACK")]
        [SerializeField] private TweenPunchConfig _specialAttackScalePunch;
        [SerializeField] private TweenPunchConfig _specialAttackFinishScalePunch;
        [SerializeField] private TweenPunchConfig _startSpecialAttackScalePunch;
        public TweenPunchConfig SpecialAttackScalePunch => _specialAttackScalePunch;
        public TweenPunchConfig SpecialAttackFinishScalePunch => _specialAttackFinishScalePunch;
        public TweenPunchConfig StartSpecialAttackScalePunch => _startSpecialAttackScalePunch;
        
    }
}