using Popeye.Modules.WorldElements.MovableBlocks.GridMovement;
using UnityEngine;

namespace Project.Modules.WorldElements.MovableBlocks.PullableBlocks
{
    public interface IPullableBlock
    {
        bool IsMoving { get; }
        void TryPullTowardsDirection(Vector2 pullDirection);

    }
}