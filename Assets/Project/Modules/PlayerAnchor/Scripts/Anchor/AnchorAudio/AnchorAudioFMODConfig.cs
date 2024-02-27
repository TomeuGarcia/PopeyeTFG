using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "AnchorAudioFMODConfig", 
        menuName = ScriptableObjectsHelper.ANCHOR_ASSETS_PATH + "AnchorAudioFMODConfig")]
    public class AnchorAudioFMODConfig : ScriptableObject
    {
        [SerializeField] private OneShotFMODSound _dealDamage;
        [SerializeField] private OneShotFMODSound _throw;
        [SerializeField] private OneShotFMODSound _grab;
        
        public OneShotFMODSound DealDamage => _dealDamage;
        public OneShotFMODSound Throw => _throw;
        public OneShotFMODSound Grab => _grab;

    }
}