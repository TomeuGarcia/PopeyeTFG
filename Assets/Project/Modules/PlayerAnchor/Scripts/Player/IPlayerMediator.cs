using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IPlayerMediator
    {
        Vector3 Position { get; }

        void SetMaxMovementSpeed(float maxMovementSpeed);
        void SetCanUseRotateInput(bool canUseRotateInput);
        void SetCanRotate(bool canRotate);
        float GetDistanceFromAnchor();
        float GetDistanceFromAnchorRatio01();
        Vector3 GetFloorAlignedDirectionToAnchor();
        Vector3 GetLookDirection();
        Vector3 GetFloorAlignedLookDirection();
        Vector3 GetLookDirectionConsideringSteep();
        Vector3 GetFloorNormal();
        Vector3 GetAnchorThrowStartPosition();



        void PickUpAnchor();
        void StartChargingThrow();
        void ChargeThrow(float deltaTime);
        void StopChargingThrow();
        void CancelChargingThrow();
        void ThrowAnchor();
        void PullAnchor();
        void OnPullAnchorComplete();
        
        UniTask DashTowardsAnchor();
        UniTask DashForward();
        void KickAnchor();
        UniTaskVoid StartSpinningAnchor();
        void StopSpinningAnchor();


        void OnAnchorEndedInVoid();


        UniTaskVoid LookTowardsAnchorForDuration(float duration);
        

        bool HasStaminaLeft();
        bool HasMaxStamina();


        bool CanHeal();
        UniTask UseHeal();
        void HealToMax();
        
        void OnDamageTaken();
        void OnKilledByDamageTaken();
        void OnHealed();


        Transform GetTargetForEnemies();
        void Respawn();
    }
}