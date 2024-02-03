using Popeye.InverseKinematics.Bones;
using UnityEngine;

namespace Popeye.InverseKinematics.FABRIK
{
    public class FABRIKInstaller : MonoBehaviour
    {
        [SerializeField] private FABRIKControllerBehaviour _FABRIKControllerBehaviour;
        [SerializeField] private BoneChain _boneChain;
        [SerializeField] private Transform _target;


        private void Awake()
        {
            _boneChain.AwakeInit();
            _FABRIKControllerBehaviour.AwakeInit(_boneChain, _target);
        }

    }
}
