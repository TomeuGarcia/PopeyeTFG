using System;
using UnityEngine;

namespace Popeye.Modules.WorldElements.WorldBuilders
{
    public class WallBuilder : MonoBehaviour
    {
        public Vector3[] points = { 
            Vector3.zero, 
            Vector3.right, 
            Vector3.right + Vector3.forward 
        };

        [System.Serializable]
        public struct Block
        {
            public Vector2 size;
            private Vector2 _halfSize;

            public Vector3[] ToFrame(Vector3 center, Quaternion rotation)
            {
                Vector3[] framePositions = new Vector3[4];

                framePositions[0] = center + (rotation * (Vector3.right * _halfSize.x +   Vector3.forward * _halfSize.y));
                framePositions[1] = center + (rotation * (Vector3.right * _halfSize.x +   Vector3.back * _halfSize.y));
                framePositions[2] = center + (rotation * (Vector3.left * _halfSize.x +    Vector3.back * _halfSize.y));
                framePositions[3] = center + (rotation * (Vector3.left * _halfSize.x +    Vector3.forward * _halfSize.y));

                return framePositions;
            }

            public void UpdateHalfSize()
            {
                _halfSize = size / 2;
            }
        }

        public Block cornerBlock;
        public Block fillBlock;


        private void OnValidate()
        {
            cornerBlock.UpdateHalfSize();
            fillBlock.UpdateHalfSize();
        }

        public void AddPoint()
        {
            Array.Resize(ref points, points.Length + 1);

            points[^1] = points[^2] + Vector3.right;
        }
        
    }
}