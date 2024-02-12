using UnityEngine;

namespace Popeye.Modules.WorldElements.MovableBlocks.GridMovement
{
    public static class GridMovementAreaViewHelper
    {
        public static void CreateRectangularAreaView(GridMovementAreaViewConfig  config,
            Transform parent, RectangularArea rectangularArea, Vector3 positionOffset)
        {
            GameObject meshHolder = GameObject.Instantiate(config.QuadMeshPrefab, parent);

            meshHolder.transform.position = rectangularArea.Center + positionOffset;
            meshHolder.transform.localScale =
                new Vector3(rectangularArea.AreaBounds.size.x, rectangularArea.AreaBounds.size.y, 1.0f);

            meshHolder.GetComponent<MeshRenderer>().material = config.AreaMaterial;
        }
    }
}