using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    
    [System.Serializable]
    public class PlayerAnimatorViewConfig
    {
        [Header("PARAMETERS")]
        [SerializeField] private string _idleParameter = "idle";
        [SerializeField] private string _movingWithAnchorParameter = "movingWithAnchor";
        [SerializeField] private string _movingWithoutAnchorParameter = "movingWithoutAnchor";
        [SerializeField] private string _aimingParameter = "aiming";
        [SerializeField] private string _throwingAnchorParameter = "throwing";
        [SerializeField] private string _pullingAnchorParameter = "pulling";
        [SerializeField] private string _pickUpAnchorParameter = "pickUpAnchor";
        public string IdleParameter => _idleParameter;
        public string MovingWithAnchorParameter => _movingWithAnchorParameter;
        public string MovingWithoutAnchorParameter => _movingWithoutAnchorParameter;
        public string AimingParameter => _aimingParameter;
        public string ThrowingAnchorParameter => _throwingAnchorParameter;
        public string PullingAnchorParameter => _pullingAnchorParameter;
        public string PickUpAnchorParameter => _pickUpAnchorParameter;
        
        
        [Header("LAYERS")]
        [SerializeField] private string _legsLayer = "Legs Layer";
        public string LegsLayer => _legsLayer;
    }
}