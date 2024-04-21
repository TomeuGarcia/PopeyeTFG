using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerController.Inputs
{
    [CreateAssetMenu(fileName = "PlayerMovesetInputsConfig", 
        menuName = ScriptableObjectsHelper.PLAYERINPUTS_ASSETS_PATH + "PlayerMovesetInputsConfig")]
    public class PlayerMovesetInputsConfig : ScriptableObject
    {
        [System.Serializable]
        private class InputBufferDurations
        {
            [SerializeField, Range(0f, 5.0f)] private float _aim = 0.2f;
            [SerializeField, Range(0f, 5.0f)] private float _throw = 0.2f;
            [SerializeField, Range(0f, 5.0f)] private float _pull = 0.2f;
            [SerializeField, Range(0f, 5.0f)] private float _dashTowardsAnchor = 0.2f;
            
            public float Aim => _aim;
            public float Throw => _throw;
            public float Pull => _pull;
            public float DashTowardsAnchor => _dashTowardsAnchor;
        }

        [Header("INPUT BUFFERS")] 
        [SerializeField] private InputBufferDurations _inputBufferDurations;

        public float AimInputBufferDuration => _inputBufferDurations.Aim;
        public float ThrowInputBufferDuration => _inputBufferDurations.Throw;
        public float PullInputBufferDuration => _inputBufferDurations.Pull;
        public float DashTowardsAnchorInputBufferDuration => _inputBufferDurations.DashTowardsAnchor;
    }
}