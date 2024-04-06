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
        [Expandable] [SerializeField] private LastingFMODSound _footstepsSound;
        [Expandable] [SerializeField] private OneShotFMODSound _dashTowardsAnchorSound;
        [Expandable] [SerializeField] private OneShotFMODSound _dashDroppingAnchor;
        [Expandable] [SerializeField] private OneShotFMODSound _takeDamage;
        
        public LastingFMODSound FootstepsSound => _footstepsSound;
        public OneShotFMODSound DashTowardsAnchorSound => _dashTowardsAnchorSound;
        public OneShotFMODSound DashDroppingAnchor => _dashDroppingAnchor;
        public OneShotFMODSound TakeDamage => _takeDamage;
    }
}