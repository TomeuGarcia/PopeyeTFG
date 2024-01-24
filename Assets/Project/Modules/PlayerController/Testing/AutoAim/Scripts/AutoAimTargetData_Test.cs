using Popeye.Modules.PlayerController.AutoAim;
using UnityEngine;

namespace Project.Modules.PlayerController.Testing.AutoAim.Scripts
{
    public class AutoAimTargetData_Test : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        public float AngularPosition { get; private set; }
        
        public float HalfAngularSize => _config.HalfAngularSize;
        public float HalfAngularTargetRegion => _config.HalfAngularTargetRegion;
        public float HalfFlatCenterAngularTargetRegion => _config.HalfFlatCenterAngularTargetRegion;
        
        

        
        [SerializeField] private AutoAimTargetDataConfig _config;
        
        [SerializeField] private Transform _helpViewer;
        [SerializeField] private Transform _helpViewerA;
        [SerializeField] private Transform _helpViewerB;
        public Transform HelpViewer => _helpViewer;
        public Transform HelpViewerA => _helpViewerA;
        public Transform HelpViewerB => _helpViewerB;


        public void SetAngleAtCenter(float angle)
        {
            AngularPosition = angle;
        }
    }
}