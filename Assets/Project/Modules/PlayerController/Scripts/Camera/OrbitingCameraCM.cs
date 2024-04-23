using System;
using Cinemachine;
using UnityEngine;

namespace Popeye.Modules.Camera
{
    
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class OrbitingCameraCM : MonoBehaviour, ICameraController
    {
        [SerializeField] private CinemachineVirtualCamera _cmVirtualCamera;
        private Cinemachine3rdPersonFollow _cmBodyComponent;

        public CinemachineVirtualCamera VirtualCamera => _cmVirtualCamera;

        public float Distance => _cmBodyComponent.CameraDistance;
        public float DefaultDistance { get; private set; }
        public Transform CameraTransform => transform;


        private void Awake()
        {
            _cmBodyComponent = (Cinemachine3rdPersonFollow)_cmVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);

            DefaultDistance = Distance;
        }

        public void SetDistance(float distance)
        {
            _cmBodyComponent.CameraDistance = distance;
        }
        
        
    }
}