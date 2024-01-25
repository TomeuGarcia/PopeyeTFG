using Popeye.Modules.PlayerController.AutoAim;
using UnityEngine;

namespace Project.Modules.PlayerController.Testing.AutoAim.Scripts
{
    public class AutoAimTargetData_Test : MonoBehaviour, IAutoAimTarget
    {
        public AutoAimTargetDataConfig DataConfig => _config;
        public Vector3 Position => transform.position;
        public GameObject GameObject => gameObject;
        public bool CanBeAimedAt(Vector3 aimFromPosition)
        {
            return true;
        }
        
        
        public float HalfAngularSize => _config.HalfAngularSize;
        public float HalfAngularTargetRegion => _config.HalfAngularTargetRegion;
        

        
        [SerializeField] private AutoAimTargetDataConfig _config;
        
        [SerializeField] private Transform _helpViewer;
        [SerializeField] private Transform _helpViewerA;
        [SerializeField] private Transform _helpViewerB;
        public Transform HelpViewer => _helpViewer;
        public Transform HelpViewerA => _helpViewerA;
        public Transform HelpViewerB => _helpViewerB;


    }
}