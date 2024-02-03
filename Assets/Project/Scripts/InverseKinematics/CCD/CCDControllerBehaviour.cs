using Popeye.InverseKinematics.Bones;
using Popeye.InverseKinematics.FABRIK;
using UnityEngine;

namespace Project.Scripts.InverseKinematics.CCD
{
    public class CCDControllerBehaviour : MonoBehaviour
    {
        private CCDController _controller;
        private BoneChain _boneChain;
        private Transform _target;
        
        private Vector3 _clampedAnglesMin;
        private Vector3 _clampedAnglesMax;
        private Vector3 _clampAxis;


        private void OnDestroy()
        {
            _boneChain.OnGenerationUpdate -= ResetController;
        }
        
        public void AwakeInit(BoneChain boneChain, Transform target, 
            Vector3 clampedAnglesMin, Vector3 clampedAnglesMax, Vector3 clampAxis)
        {
            _boneChain = boneChain;
            _target = target;
            _clampedAnglesMin = clampedAnglesMin;
            _clampedAnglesMax = clampedAnglesMax;
            _clampAxis = clampAxis;
            
            _controller = new CCDController();
            ResetController();

            _boneChain.OnGenerationUpdate += ResetController;
        }


        private void Update()
        {
            _controller.Update();
        }


        private void ResetController()
        {
            _controller.RemoveJointChains();
            _controller.AddJointChain(
                new CCDJointChain(BoneChainHelper.JointChainFromBoneChain(_boneChain), _target,
                    _clampedAnglesMin, _clampedAnglesMax, _clampAxis));
        }
    }
}