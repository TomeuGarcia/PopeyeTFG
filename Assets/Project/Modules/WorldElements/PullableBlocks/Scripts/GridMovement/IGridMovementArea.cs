using UnityEngine;

namespace Popeye.Modules.WorldElements.PullableBlocks.GridMovement
{
    public interface IGridMovementArea
    {
        bool CanMoveTowardsDirection(Rect actorBounds, Vector2 movementDisplacement);
    }
}