using Popeye.InverseKinematics.Bones;
using UnityEngine;

namespace Popeye.InverseKinematics.FABRIK
{
    public class FABRIKControllerBehaviour : MonoBehaviour
    {
        private FABRIKController _controller;
        private BoneChain _boneChain;
        private Transform _target;


        private void OnDestroy()
        {
            _boneChain.OnGenerationUpdate -= ResetFABRIKController;
        }
        
        public void AwakeInit(BoneChain boneChain, Transform target)
        {
            _boneChain = boneChain;
            _target = target;
            _controller = new FABRIKController();
            ResetFABRIKController();

            _boneChain.OnGenerationUpdate += ResetFABRIKController;
        }


        private void Update()
        {
            _controller.Update();
        }


        private void ResetFABRIKController()
        {
            _controller.RemoveJointChains();
            _controller.AddJointChain(BoneChainHelper.FABRIKJointChainFromBoneArm(_boneChain, _target));
        }
    }
}