using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorSnapTarget
    {
        public Vector3 GetSnapPosition();
        public Quaternion GetSnapRotation();

        public bool CanSnapFromPosition(Vector3 position);
        
        public void EnterPrepareForSnapping();
        public void QuitPrepareForSnapping();
        public UniTaskVoid PlaySnapAnimation(float delay);
    }
}