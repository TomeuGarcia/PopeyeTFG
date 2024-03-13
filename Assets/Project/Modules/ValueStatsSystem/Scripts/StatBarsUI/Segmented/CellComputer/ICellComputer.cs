using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Modules.ValueStatSystem.Segmented
{
    public interface ICellComputer
    {
        Vector2 ComputeCellSize(int numberOfSegments, Rect holderRect, GridLayoutGroup gridLayoutGroup);
        Vector2 ComputeSpacingBetweenCells(int numberOfSegments, Rect holderRect, GridLayoutGroup gridLayoutGroup);
        RectOffset ComputePaddingCells(Rect holderRect, GridLayoutGroup gridLayoutGroup);
    }
}