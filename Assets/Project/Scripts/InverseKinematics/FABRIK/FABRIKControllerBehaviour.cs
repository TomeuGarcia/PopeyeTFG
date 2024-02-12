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
            _boneChain.OnGenerationUpdate -= ResetController;
        }
        
        public void StartInit(BoneChain boneChain, Transform target)
        {
            _boneChain = boneChain;
            _target = target;
            _controller = new FABRIKController();
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
                new FABRIKJointChain(BoneChainHelper.JointChainFromBoneChain(_boneChain), _target));
        }
    }
}