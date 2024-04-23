using NaughtyAttributes;
using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    [CreateAssetMenu(fileName = "PlayerAudioFMODConfig", 
        menuName = ScriptableObjectsHelper.PLAYER_ASSETS_PATH + "PlayerAudioFMODConfig")]
    public class PlayerAudioFMODConfig : ScriptableObject
    {
        [Header("FOOTSTEPS")]
        [Expandable] [SerializeField] private OneShotFMODSound _leftFootstepSound;
        [Expandable] [SerializeField] private OneShotFMODSound _rightFootstepSound;
        [Expandable] [SerializeField] private LastingFMODSound _footstepsSound;
        
        [Header("DASH")]
        [Expandable] [SerializeField] private OneShotFMODSound _dashTowardsAnchorSound;
        [Expandable] [SerializeField] private OneShotFMODSound _dashDroppingAnchor;
        
        [Header("TAKE DAMAGE")]
        [Expandable] [SerializeField] private OneShotFMODSound _takeDamage;
        
        
        
        public LastingFMODSound FootstepsSound => _footstepsSound;
        public OneShotFMODSound LeftFootstepSound => _leftFootstepSound;
        public OneShotFMODSound RightFootstepSound => _rightFootstepSound;
        
        public OneShotFMODSound DashTowardsAnchorSound => _dashTowardsAnchorSound;
        public OneShotFMODSound DashDroppingAnchor => _dashDroppingAnchor;
        
        public OneShotFMODSound TakeDamage => _takeDamage;
    }
}