using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IPlayerMediator
    {

        public void SetMaxMovementSpeed(float maxMovementSpeed);
        public void SetCanRotate(bool canRotate);
        public float GetDistanceFromAnchor();
        public Vector3 GetFloorAlignedLookDirection();
        public Vector3 GetFloorNormal();
        public Vector3 GetAnchorThrowStartPosition();



        public void CarryAnchor();
        public void StartChargingThrow();
        public void ChargeThrow(float deltaTime);
        public void StopChargingThrow();
        public void CancelChargingThrow();
        public void ThrowAnchor();
        public void PullAnchor();


        public void OnAnchorThrowEndedInVoid();
    }
}