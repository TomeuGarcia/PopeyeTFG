using Popeye.InverseKinematics.Bones;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class BoneChainChainViewLogic : IChainViewLogic
    {
        private readonly int _chainBoneCount;

        private readonly Vector3[] _chainPositions;
        
        private readonly Transform _chainIK;
        private readonly BoneChain _boneChain;
        


        public BoneChainChainViewLogic(int chainBoneCount, Transform chainIK, BoneChain boneChain)
        {
            _chainBoneCount = chainBoneCount;
            _chainIK = chainIK;
            _boneChain = boneChain;
            
            _chainPositions = new Vector3[_chainBoneCount];

            _chainIK.gameObject.SetActive(false);
        }

        public void OnViewEnter()
        {
            //_chainIK.gameObject.SetActive(false);
        }

        public void UpdateChainPositions(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            _chainPositions[0] = anchorBindPosition;
            
            for (int i = 1; i < _chainBoneCount; ++i)
            {
                _chainPositions[i] = _boneChain.Bones[i].Position;
            }
        }

        public void OnViewExit()
        {
            //_chainIK.gameObject.SetActive(false);
        }

        public Vector3[] GetChainPositions()
        {
            return _chainPositions;
        }
    }
}