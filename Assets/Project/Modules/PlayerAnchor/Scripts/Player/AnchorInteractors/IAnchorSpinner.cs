using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IAnchorSpinner
    {
        Vector3 SpinPosition { get; }
        
        public bool CanSpinningAnchor();
        public void StartSpinningAnchor(bool startsCarryingAnchor);
        public void StopSpinningAnchor();
        public void SpinAnchor(float deltaTime);
    }
}