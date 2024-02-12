using UnityEngine;

namespace Popeye.Modules.WorldElements.MovableBlocks.GridMovement
{
    public class GridMovementAreaView
    {
        private readonly GridMovementAreaViewConfig _config;

        public GridMovementAreaView(GridMovementAreaViewConfig config)
        {
            _config = config;
        }

        public void CreateRectangularAreaView(Transform parent, RectangularArea rectangularArea,
            Vector3 positionOffset)
        {
            GameObject meshHolder = GameObject.Instantiate(_config.QuadMeshPrefab, parent);

            meshHolder.transform.position = rectangularArea.Center + positionOffset;
            meshHolder.transform.localScale =
                new Vector3(rectangularArea.AreaBounds.size.x, rectangularArea.AreaBounds.size.y, 1.0f);

            meshHolder.GetComponent<MeshRenderer>().material = _config.AreaMaterial;
        }
    }
}