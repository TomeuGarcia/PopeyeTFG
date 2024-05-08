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

        public Vector2 ComputeGeneralHolderSize(int numberOfSegments, GridLayoutGroup gridLayoutGroup, RectTransform holder)
        {
            float width = gridLayoutGroup.padding.horizontal + (gridLayoutGroup.cellSize.x * numberOfSegments);
            float height = gridLayoutGroup.padding.vertical + gridLayoutGroup.cellSize.y;
                
            return new Vector2(width, height);     
        }
    }
}