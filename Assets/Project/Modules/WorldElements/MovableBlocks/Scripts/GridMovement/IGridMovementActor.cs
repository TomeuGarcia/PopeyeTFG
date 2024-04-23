using UnityEngine;

namespace Popeye.Modules.WorldElements.MovableBlocks.GridMovement
{
    public interface IGridMovementActor
    {
        RectangularArea RectangularArea { get; }
        Rect AreaBounds { get; }
        void Configure(GridMovementArea associatedMovementArea);
    }
}