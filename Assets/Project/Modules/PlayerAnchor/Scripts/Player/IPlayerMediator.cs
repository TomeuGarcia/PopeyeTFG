using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Player.Stamina;
using Project.Modules.WorldElements.DestructiblePlatforms;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IPlayerMediator
    {
        Vector3 Position { get; }
        Transform PositionTransform { get; }
        IPlayerView PlayerView { get; }
        IPlayerHealing PlayerHealing { get; }
        IPlayerStaminaPower PlayerStaminaPower { get; }
        DestructiblePlatformBreaker DestructiblePlatformBreaker { get; }

        void SetMaxMovementSpeed(float maxMovementSpeed);
        void SetCanUseRotateInput(bool canUseRotateInput);
        void SetInstantRotation(bool instantRotation);
        void SetCanRotate(bool canRotate);
        void SetCanFallOffLedges(bool canFallOffLedges, bool checkingIgnoreLedges = true);
        float GetDistanceFromAnchor();
        float GetDistanceFromAnchorRatio01();
        Vector3 GetFloorAlignedDirectionToAnchor();
        Vector3 GetLookDirection();
        Vector3 GetRightDirection();
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
        UniTaskVoid QueuePullAnchor();
        
        UniTask DashTowardsAnchor();
        UniTask DashForward();
        void KickAnchor();
        
        bool CanSpinAnchor();
        bool IsLockedIntoSpinningAnchor();
        void StartSpinningAnchor(bool startsCarryingAnchor, bool spinToTheRight);
        void SpinAnchor(float deltaTime);
        void StopSpinningAnchor();
        void InterruptSpinningAnchor();
        bool SpinningAnchorFinished();


        void OnAnchorEndedInVoid();
        void OnPlayerFellOnVoid();
        bool TakeFellOnVoidDamage();
        void RespawnToLastSafeGround();
        void OnTryUsingObstructedAnchor();


        void LookTowardsPosition(Vector3 position);
        void LookTowardsAnchor();
        UniTaskVoid LookTowardsAnchorForDuration(float duration);


        void DropTargetForCamera();
        void ResetTargetForCamera();
        

        bool HasStaminaLeft();
        bool HasMaxStamina();


        void SetInvulnerable(bool isInvulnerable);
        void SetInvulnerableForDuration(float duration);

        void OnDamageTaken();
        void OnKilledByDamageTaken();
        void OnHealed();
        void OnHealStart(float durationToComplete);
        void OnHealInterrupted();

        Transform GetTargetForEnemies();
        void RespawnFromDeath();


        void OnStartMoving();
        void OnStopMoving();


        void UpdateSafeGroundChecking(float deltaTime, out bool playerIsOnVoid, out bool anchorIsOnVoid);

    }
}