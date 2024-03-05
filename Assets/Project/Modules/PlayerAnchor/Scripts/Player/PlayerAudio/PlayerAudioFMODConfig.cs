using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    [CreateAssetMenu(fileName = "PlayerAudioFMODConfig", 
        menuName = ScriptableObjectsHelper.PLAYER_ASSETS_PATH + "PlayerAudioFMODConfig")]
    public class PlayerAudioFMODConfig : ScriptableObject
    {
        [SerializeField] private LastingFMODSound _footstepsSound;
        
        public LastingFMODSound FootstepsSound => _footstepsSound;
    }
}