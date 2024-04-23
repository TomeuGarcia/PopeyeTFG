using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    
    [System.Serializable]
    public class PlayerAnimatorViewConfig
    {
        [Header("PARAMETERS")]
        [SerializeField] private string _idleParameter = "idle";
        [SerializeField] private string _movingWithAnchorParameter = "movingWithAnchor";
        [SerializeField] private string _fMovingWithAnchorParameter = "fMovingWithAnchor";
        [SerializeField] private string _movingWithoutAnchorParameter = "movingWithoutAnchor";
        [SerializeField] private string _aimingParameter = "aiming";
        [SerializeField] private string _throwingAnchorParameter = "throwing";
        [SerializeField] private string _pullingAnchorParameter = "pulling";
        [SerializeField] private string _pickUpAnchorParameter = "pickUpAnchor";
        [SerializeField] private string _tiredParameter = "tired";
        [SerializeField] private string _idleToMovingParameter = "idleToMovingBlending";

        public int IdleParameterId { get; private set; }
        public int MovingWithAnchorParameterId { get; private set; }
        public int FMovingWithAnchorParameterId { get; private set; }
        public int MovingWithoutAnchorParameterId { get; private set; }
        public int AimingParameterId { get; private set; }
        public int ThrowingAnchorParameterId { get; private set; }
        public int PullingAnchorParameterId { get; private set; }
        public int PickUpAnchorParameterId { get; private set; }
        public int TiredParameterId { get; private set; }
        public int IdleToMovingParameterId { get; private set; }


        public void OnValidate()
        {
                    
            IdleParameterId = Animator.StringToHash(_idleParameter);
            MovingWithAnchorParameterId = Animator.StringToHash(_movingWithAnchorParameter);
            FMovingWithAnchorParameterId = Animator.StringToHash(_fMovingWithAnchorParameter);
            MovingWithoutAnchorParameterId = Animator.StringToHash(_movingWithoutAnchorParameter);
            AimingParameterId = Animator.StringToHash(_aimingParameter);
            ThrowingAnchorParameterId = Animator.StringToHash(_throwingAnchorParameter);
            PullingAnchorParameterId = Animator.StringToHash(_pullingAnchorParameter);
            PickUpAnchorParameterId = Animator.StringToHash(_pickUpAnchorParameter);
            TiredParameterId = Animator.StringToHash(_tiredParameter);
            IdleToMovingParameterId = Animator.StringToHash(_idleToMovingParameter);
        }
    }
}