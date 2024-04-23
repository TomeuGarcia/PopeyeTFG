using UnityEngine;

namespace Popeye.InverseKinematics.Bones
{
    public static class BoneChainHelper
    {
        public static Transform[] JointChainFromBoneChain(BoneChain boneChain)
        {
            Bone[] bones = boneChain.Bones;

            Transform[] joints = new Transform[bones.Length];
            for (int i = 0; i < bones.Length; ++i)
            {
                joints[i] = bones[i].BoneRoot;
            }

            return joints;
        }

    }
    
}