using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Modules.ValueStatSystem.Segmented
{
    public class FixedSizeCellComputer : ICellComputer
    {
        public FixedSizeCellComputer()
        {
        }
        
        
        public Vector2 ComputeCellSize(int numberOfSegments, Rect holderRect, GridLayoutGroup gridLayoutGroup)
        {
            return gridLayoutGroup.cellSize;
        }

        public Vector2 ComputeSpacingBetweenCells(int numberOfSegments, Rect holderRect, GridLayoutGroup gridLayoutGroup)
        {
            return gridLayoutGroup.spacing;
        }
        
        public RectOffset ComputePaddingCells(Rect holderRect, GridLayoutGroup gridLayoutGroup)
        {
            return gridLayoutGroup.padding;
        }
    }
}