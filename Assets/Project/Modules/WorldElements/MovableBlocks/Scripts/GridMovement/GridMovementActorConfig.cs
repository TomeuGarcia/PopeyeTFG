using DG.Tweening;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.WorldElements.MovableBlocks.GridMovement
{
    
    [CreateAssetMenu(fileName = "NAME_GridMovementActorConfig", 
        menuName = ScriptableObjectsHelper.GRIDMOVEMENT_ASSETS_PATH + "GridMovementActorConfig")]
    public class GridMovementActorConfig : ScriptableObject
    {
        [SerializeField, Range(1, 10)] private int _moveAmount = 2;
        [SerializeField, Range(0f, 2f)] private float _moveDuration = 0.2f;
        [SerializeField] private Ease _moveEase = Ease.InOutSine;
        
        [SerializeField, Range(0f, 2f)] private float _delayBetweenMoves = 0.1f;
        

        public int MoveAmount => _moveAmount;
        public float MoveDuration => _moveDuration;
        public Ease MoveEase => _moveEase;
        public float DelayBetweenMoves => _delayBetweenMoves;
    }
}