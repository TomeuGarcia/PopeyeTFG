using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    [CreateAssetMenu(fileName = "HazardDispenserConfig_NAME", 
        menuName = ScriptableObjectsHelper.HAZARDS_ASSET_PATH + "HazardDispenserConfig")]
    public class HazardDispenserConfig : ScriptableObject
    {
        [Header("LOGIC")]
        [SerializeField, Range(0f, 10.0f)] private float _delayBeforeDispensing = 0.0f;
        [SerializeField, Range(0f, 10.0f)] private float _telegraphBeforeDispensing = 0.5f;
        [SerializeField, Range(0f, 10.0f)] private float _cooldownAfterDispensing = 2.0f;

        public float DelayBeforeDispensing => _delayBeforeDispensing;
        public float TelegraphBeforeDispensing => _telegraphBeforeDispensing;
        public float CooldownAfterDispensing => _cooldownAfterDispensing;


        [Header("VIEW")]
        [SerializeField] private HazardDispenserViewConfig _viewConfig;
        public HazardDispenserViewConfig ViewConfig => _viewConfig;


        [Header("AUDIO")] 
        [SerializeField] private FMODHazardDispenserAudio _hazardDispenserAudio;
        public IHazardDispenserAudio HazardDispenserAudio => _hazardDispenserAudio;
    }
}