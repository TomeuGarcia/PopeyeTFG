using UnityEngine;

namespace Project.Modules.PlayerController.Testing.AutoAim.Scripts
{
    public class AutoAimTargetData_Test : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        public float AngleAtCenter { get; private set; }
        public float AngleSize => 20f;
        public float HalfAngleSize => AngleSize / 2;

        
        [SerializeField] private Transform _helpViewer;
        [SerializeField] private Transform _helpViewerA;
        [SerializeField] private Transform _helpViewerB;
        public Transform HelpViewer => _helpViewer;
        public Transform HelpViewerA => _helpViewerA;
        public Transform HelpViewerB => _helpViewerB;


        public void SetAngleAtCenter(float angle)
        {
            AngleAtCenter = angle;
        }
    }
}