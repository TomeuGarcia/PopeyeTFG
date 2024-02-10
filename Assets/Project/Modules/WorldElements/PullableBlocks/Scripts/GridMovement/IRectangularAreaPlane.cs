using UnityEngine;

namespace Popeye.Modules.WorldElements.PullableBlocks.GridMovement
{
    public interface IRectangularAreaPlane
    {
        Vector3 Center { get; set; }
        Vector3 CornerA { get; set; }
        Vector3 CornerB { get; set; }
        Vector3 CornerC { get; set; }
        Vector3 CornerD { get; set; }
    }
}