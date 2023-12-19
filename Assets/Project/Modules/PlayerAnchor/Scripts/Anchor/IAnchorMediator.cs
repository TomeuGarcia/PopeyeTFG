
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorMediator
    {
        Vector3 Position { get; }

        void SetPosition(Vector3 position);
        void SetRotation(Quaternion rotation);
        
        bool IsBeingThrown();
        bool IsBeingPulled();
        bool IsRestingOnFloor();

        bool IsGrabbedBySnapper();


        void SetSpinning(bool spinningToTheRight);
        void OnKeepSpinning();
        void OnStopSpinning();
        
        UniTaskVoid SnapToFloor(Vector3 noFloorAlternativePosition);

        bool IsObstructedByObstacles();
        
        void SubscribeToOnObstacleHit(Action<Collider> callback);
        void UnsubscribeToOnObstacleHit(Action<Collider> callback);
        void EnableObstacleHitForDuration(float duration);

        void OnTryUsingWhenObstructed();


        void OnDamageDealt();

    }
}