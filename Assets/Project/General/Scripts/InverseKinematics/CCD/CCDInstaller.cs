using Popeye.InverseKinematics.Bones;
using UnityEngine;

namespace Project.Scripts.InverseKinematics.CCD
{
    public class CCDInstaller : MonoBehaviour
    {
        [SerializeField] private CCDControllerBehaviour _CCDControllerBehaviour;
        [SerializeField] private BoneChain _boneChain;
        [SerializeField] private Transform _target;
        
        [SerializeField] private Vector3 _clampedAnglesMin = new Vector3(-20, -6, 0);
        [SerializeField] private Vector3 _clampedAnglesMax = new Vector3(-20, 6, 0);
        [SerializeField] private Vector3 _clampAxis = Vector3.forward;

        private void Awake()
        {
            _boneChain.StartInit();
            _CCDControllerBehaviour.StartInit(_boneChain, _target, _clampedAnglesMin, _clampedAnglesMax, _clampAxis);
        }

    }
}