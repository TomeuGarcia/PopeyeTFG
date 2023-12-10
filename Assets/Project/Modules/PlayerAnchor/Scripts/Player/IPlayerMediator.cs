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
        public Vector3 GetFloorAlignedDirectionToAnchor();
        public Vector3 GetLookDirection();
        public Vector3 GetFloorAlignedLookDirection();
        public Vector3 GetLookDirectionConsideringSteep();
        public Vector3 GetFloorNormal();
        public Vector3 GetAnchorThrowStartPosition();



        public void PickUpAnchor();
        public void StartChargingThrow();
        public void ChargeThrow(float deltaTime);
        public void StopChargingThrow();
        public void CancelChargingThrow();
        public void ThrowAnchor();
        public void PullAnchor();
        public void OnPullAnchorComplete();
        public void DashTowardsAnchor(float duration);
        public void KickAnchor();


        public void OnAnchorEndedInVoid();


        public UniTaskVoid LookTowardsAnchorForDuration(float duration);
        

        public bool HasStaminaLeft();
        public bool HasMaxStamina();


        public bool CanHeal();
        public UniTask UseHeal();
        public void HealToMax();
        
        public void OnDamageTaken();
        public void OnKilledByDamageTaken();
        public void OnHealed();


        public Transform GetTargetForEnemies();
        public void Respawn();
    }
}