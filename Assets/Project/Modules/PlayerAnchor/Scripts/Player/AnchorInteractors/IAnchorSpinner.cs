using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IAnchorSpinner
    {
        Vector3 SpinCircumferencePosition { get; }
        
        public bool CanSpinningAnchor();
        public bool IsLockedIntoSpinningAnchor();
        public bool SpinningAnchorFinished();
        public void StartSpinningAnchor(bool startsCarryingAnchor, bool spinToTheRight);
        public void StopSpinningAnchor();
        public void SpinAnchor(float deltaTime);
        public void InterruptSpinningAnchor();
    }
}