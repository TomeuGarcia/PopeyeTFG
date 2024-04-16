using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    [System.Serializable]
    public class HazardDispenserViewConfig
    {
        [Header("PREPARE")]
        [SerializeField] private TweenConfig _prepareRotation;
        [SerializeField] private TweenConfig _prepareScale;
        
        [Header("DISPENSE")]
        [SerializeField] private TweenPunchConfig _dispenseScalePunch;
        
        [Header("READY")]
        [SerializeField] private TweenPunchConfig _readyRotationPunch;
        
        
        public TweenConfig PrepareRotation => _prepareRotation;
        public TweenConfig PrepareScale => _prepareScale;
        public TweenPunchConfig DispenseScalePunch => _dispenseScalePunch;
        public TweenPunchConfig ReadyRotationPunch => _readyRotationPunch;
    }
}