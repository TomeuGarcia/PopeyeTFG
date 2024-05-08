using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.WorldElements.AnchorTriggerables
{
    [CreateAssetMenu(fileName = "TimerPressurePlateAudios", 
        menuName = ScriptableObjectsHelper.WORLDELEMENTS_ASSETS_PATH + "TimerPressurePlateAudios")]
    public class TimerPressurePlateAudios : ScriptableObject
    {
        [Header("AUDIO MANAGER")]
        [SerializeField] private AFMODAudioManagerReference _audioManagerReference;

        [Header("SOUNDS")] 
        [SerializeField] private OneShotFMODSound _activatedSound;
        
        
        [Header("TICK SOUNDS")]
        [SerializeField] private OneShotFMODSound _tickDownSound;
        [SerializeField] private OneShotFMODSound _fastTickDownSound;
        

        public void PlayActivatedSound(GameObject source)
        {
            _audioManagerReference.PlayOneShotAttached(_activatedSound, source);
        }
        
        public void PlayTickSound(GameObject source)
        {
            _audioManagerReference.PlayOneShotAttached(_tickDownSound, source);
        }
        
        public void PlayFastTickSound(GameObject source)
        {
            _audioManagerReference.PlayOneShotAttached(_fastTickDownSound, source);
        }
        
        
    }
}