using NaughtyAttributes;
using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "AnchorAudioFMODConfig", 
        menuName = ScriptableObjectsHelper.ANCHOR_ASSETS_PATH + "AnchorAudioFMODConfig")]
    public class AnchorAudioFMODConfig : ScriptableObject
    {
        [Expandable] [SerializeField] private OneShotFMODSound _dealDamage;
        [Expandable] [SerializeField] private OneShotFMODSound _throw;
        [Expandable] [SerializeField] private OneShotFMODSound _grab;
        [Expandable] [SerializeField] private OneShotFMODSound _pull;
        [Expandable] [SerializeField] private OneShotFMODSound _landOnFloor;
        
        public OneShotFMODSound DealDamage => _dealDamage;
        public OneShotFMODSound Throw => _throw;
        public OneShotFMODSound Grab => _grab;
        public OneShotFMODSound Pull => _pull;
        public OneShotFMODSound LandOnFloor => _landOnFloor;

    }
}