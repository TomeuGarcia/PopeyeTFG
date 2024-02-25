
using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.CombatSystem;
using DG.Tweening;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking.OnVoid;
using Project.Modules.WorldElements.DestructiblePlatforms;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorMediator
    {
        Transform PositionTransform { get; }
        Vector3 Position { get; }
        IAnchorTrajectorySnapTarget CurrentTrajectorySnapTarget { get; }
        DestructiblePlatformBreaker DestructiblePlatformBreaker { get; }
        
        IOnVoidChecker OnVoidChecker { get; }

        void SetPosition(Vector3 position);
        void SetRotation(Quaternion rotation);
        
        bool IsBeingThrown();
        bool IsBeingPulled();
        bool IsRestingOnFloor();

        bool IsGrabbedBySnapper();


        void SetSpinning(bool spinningToTheRight);
        void OnKeepSpinning();
        void OnStopSpinning();
        
        void OnDashedAt(float duration, Ease dashEase);
        void OnDashedAwayFrom(float duration, Ease dashEase);
        
        UniTaskVoid SnapToFloor(Vector3 noFloorAlternativePosition);

        bool IsObstructedByObstacles();
        
        void SubscribeToOnObstacleHit(Action<Collider> callback);
        void UnsubscribeToOnObstacleHit(Action<Collider> callback);
        void EnableObstacleHitForDuration(float duration);

        void OnTryUsingWhenObstructed();


        void OnDamageDealt(DamageHitResult damageHitResult);

        void SetActiveDebug(bool active);
    }
}