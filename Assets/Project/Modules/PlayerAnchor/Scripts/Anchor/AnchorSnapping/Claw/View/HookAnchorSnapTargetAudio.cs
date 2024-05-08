using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "HookAnchorSnapTargetAudio", 
        menuName = ScriptableObjectsHelper.SNAPTARGETS_ASSETS_PATH + "HookAnchorSnapTargetAudio")]
    public class HookAnchorSnapTargetAudio : ScriptableObject
    {
        [SerializeField] private AFMODAudioManagerReference _audioManager;
        [SerializeField] private OneShotFMODSound _grabSound;
        [SerializeField] private OneShotFMODSound _releaseSound;
        [SerializeField] private OneShotFMODSound _jumpSound;

        public void PlayGrabSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_grabSound, source);
        }
        public void PlayReleaseSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_releaseSound, source);
        }
        public void PlayJumpSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_jumpSound, source);
        }
    }
}