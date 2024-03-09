using UnityEngine;

namespace Popeye.Modules.ValueStatSystem.Segmented
{
    public interface ICellComputer
    {
        Vector2 ComputeCellSize(int numberOfSegments, Rect holderRect);
        Vector2 ComputeSpacingBetweenCells(int numberOfSegments, Rect holderRect);
        RectOffset ComputePaddingCells(Rect holderRect);
    }
}