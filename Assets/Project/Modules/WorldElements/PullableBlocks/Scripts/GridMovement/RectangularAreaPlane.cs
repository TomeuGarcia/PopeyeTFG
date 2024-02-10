using UnityEngine;

namespace Popeye.Modules.WorldElements.PullableBlocks.GridMovement
{
    public class RectangularAreaPlane : IRectangularAreaPlane
    {
        public Vector3 Center
        {
            get;
            set;
        }

        public Vector3 CornerA
        {
            get => Center + LocalCornerA;
            set { LocalCornerA = value - Center; Debug.Log(LocalCornerA); }
        }

        public Vector3 CornerB
        {
            get => Center + LocalCornerB;
            set => LocalCornerB = value - Center;
        }
        public Vector3 CornerC
        {
            get => Center + LocalCornerC;
            set => LocalCornerC = value - Center;
        }
        public Vector3 CornerD
        {
            get => Center + LocalCornerD;
            set => LocalCornerD = value - Center;
        }

        private Vector3 LocalCornerA { get; set; }
        private Vector3 LocalCornerB { get; set; }
        private Vector3 LocalCornerC { get; set; }
        private Vector3 LocalCornerD { get; set; }
        
        

        public RectangularAreaPlane(Vector3 center, float size)
        {
            Center = center;
            size /= 2f;
            
            CornerA = Center + (Vector3.right + Vector3.forward) * size;
            CornerB = Center + (Vector3.right + Vector3.back) * size;
            CornerC = Center + (Vector3.left + Vector3.back) * size;
            CornerD = Center + (Vector3.left + Vector3.forward) * size;
        }
        
    }
}