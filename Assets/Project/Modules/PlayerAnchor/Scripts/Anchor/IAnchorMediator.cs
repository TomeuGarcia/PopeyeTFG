using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorMediator
    {
        public bool IsBeingThrown();
        public bool IsRestingOnFloor();
        public UniTaskVoid SnapToFloor();
        public void OnCollisionWithObstacle(Collision collision);
        
        
        public void OnStartChargingThrow();
        public void OnKeepChargingThrow();
        public void OnStopChargingThrow();

    }
}