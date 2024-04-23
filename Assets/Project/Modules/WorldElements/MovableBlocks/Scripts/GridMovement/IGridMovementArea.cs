using UnityEngine;

namespace Popeye.Modules.WorldElements.MovableBlocks.GridMovement
{
    public interface IGridMovementArea
    {
        bool CanMoveAfterDisplacement(IGridMovementActor gridMovementActor, Vector2 movementDisplacement);
    }
}