using UnityEngine;

namespace Project.Modules.WorldElements.MovableBlocks.PullableBlocks
{
    public interface IPullableBlock
    {
        bool IsMoving { get; }
        void TryPullTowardsDirection(Vector2 pullDirection);
        void TryPullTowardsDirectionUntilEnd(Vector2 pullDirection);

    }
}