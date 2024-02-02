using UnityEngine;

namespace Popeye.Modules.Camera
{
    public interface ICameraController
    {
        float Distance { get; }
        float DefaultDistance { get; }
        Transform CameraTransform { get; }
        
        void SetDistance(float distance);
    }
}