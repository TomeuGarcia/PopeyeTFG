
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


        void SetSpinning();
        UniTaskVoid SnapToFloor();

        void SubscribeToOnObstacleHit(Action<Collider> callback);
        void UnsubscribeToOnObstacleHit(Action<Collider> callback);

    }
}