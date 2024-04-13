using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Project.Modules.WorldElements.MovableBlocks.PullableBlocks
{
    [System.Serializable]
    public class PullableBlockViewConfig
    {
        [SerializeField] private TweenPunchConfig _moveFailedPositionPunch;
        [SerializeField, Range(0f, 5f)] private float _delayAfterFailedMove = 0.5f;
    
        public TweenPunchConfig MoveFailedPositionPunch => _moveFailedPositionPunch;
        public float DelayAfterFailedMove => _delayAfterFailedMove;
    }
}