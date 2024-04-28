using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.WorldElements.AnchorTriggerables
{
    [CreateAssetMenu(fileName = "TimerButtonInteractorAudio", 
        menuName = ScriptableObjectsHelper.WORLDELEMENTS_ASSETS_PATH + "TimerButtonInteractorAudio")]
    public class TimerButtonInteractorAudio : ScriptableObject
    {
        [Header("AUDIO MANAGER")]
        [SerializeField] private AFMODAudioManagerReference _audioManagerReference;

        [Header("SOUNDS")] 
        [SerializeField] private OneShotFMODSound _activatedSound;
        [SerializeField] private OneShotFMODSound _tickDownSound;

        public void PlayActivatedSound(GameObject source)
        {
            _audioManagerReference.PlayOneShotAttached(_activatedSound, source);
        }
        
        public void PlayTickDownSound(GameObject source)
        {
            _audioManagerReference.PlayOneShotAttached(_tickDownSound, source);
        }
    }
}