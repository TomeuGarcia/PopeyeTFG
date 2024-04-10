using UnityEngine;

namespace Project.Modules.WorldElements.MovableBlocks.PullableBlocks
{
    public interface IPullableBlock
    {
        bool IsMoving { get; }
        void TryPullTowardsDirection(Vector2 pullDirection);
        void TryPullTowardsDirectionUntilPosition(Vector2 pullDirection, Vector3 position);

    }
}