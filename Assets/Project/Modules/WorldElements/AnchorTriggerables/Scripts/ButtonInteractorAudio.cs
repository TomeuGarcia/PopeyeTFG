using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.WorldElements.AnchorTriggerables
{
    [CreateAssetMenu(fileName = "ButtonInteractorAudio", 
        menuName = ScriptableObjectsHelper.WORLDELEMENTS_ASSETS_PATH + "ButtonInteractorAudio")]
    public class ButtonInteractorAudio : ScriptableObject
    {
        [Header("AUDIO MANAGER")]
        [SerializeField] private AFMODAudioManagerReference _audioManagerReference;

        [Header("SOUNDS")] 
        [SerializeField] private OneShotFMODSound _activatedSound;

        public void PlayActivatedSound(GameObject source)
        {
            _audioManagerReference.PlayOneShotAttached(_activatedSound, source);
        }
    }
}