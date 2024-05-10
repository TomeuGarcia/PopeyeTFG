using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Modules.GameMenus.Generic.Scripts.Audio
{
    [CreateAssetMenu(fileName = "UIAudioInteractionConfig_NAME", 
        menuName = ScriptableObjectsHelper.UI_ASSETS_PATH + "UIAudioInteractionConfig")]
    public class UIAudioInteractionConfig : ScriptableObject
    {
        [SerializeField] private FMODAudioManagerReference _audioManager;
        [SerializeField] private OneShotFMODSound _interactionSound;

        public void PlaySound()
        {
            _audioManager.PlayOneShot(_interactionSound);
        }
    }
}