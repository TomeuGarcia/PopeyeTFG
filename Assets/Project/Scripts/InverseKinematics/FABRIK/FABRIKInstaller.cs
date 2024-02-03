using Popeye.InverseKinematics.Bones;
using UnityEngine;

namespace Popeye.InverseKinematics.FABRIK
{
    public class FABRIKInstaller : MonoBehaviour
    {
        [SerializeField] private BoneChain _boneChain;
        [SerializeField] private Transform _target;

        [SerializeField] private FABRIKControllerBehaviour _FABRIKControllerBehaviour;

        private void Awake()
        {
            _boneChain.AwakeInit();
            _FABRIKControllerBehaviour.AwakeInit(_boneChain, _target);
        }

    }
}
