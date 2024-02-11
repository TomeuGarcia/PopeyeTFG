using NaughtyAttributes;
using UnityEngine;

namespace Popeye.Modules.WorldElements.PullableBlocks.GridMovement
{
    public class GridMovementActorBehaviour : MonoBehaviour, IGridMovementActor
    {
        [SerializeField, Range(0f, 10f)] private float _moveAmount = 2f;
        [SerializeField] private RectangularAreaWrapper _rectangularArea;

        private GridMovementArea _associatedMovementArea;

        private Rect AreaBounds => _rectangularArea.RectangularArea.AreaBounds;


        private void OnValidate()
        {
            _rectangularArea?.OnValidateUpdateState();
        }
        private void OnDrawGizmos()
        {
            _rectangularArea?.DrawGizmos();
        }
        
        [Button("Reset Area")]
        private void ResetArea()
        {
            RectangularArea rectangularArea = new RectangularArea(transform);
            _rectangularArea = new RectangularAreaWrapper(rectangularArea);
        }


        public void Configure(GridMovementArea associatedMovementArea)
        {
            _associatedMovementArea = associatedMovementArea;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                TryMove(Vector2.left);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                TryMove(Vector2.right);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                TryMove(Vector2.up);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                TryMove(Vector2.down);
            }
        }

        private bool TryMove(Vector2 direction)
        {
            bool canMove =
                _associatedMovementArea.CanMoveTowardsDirection(AreaBounds, direction * _moveAmount);
            
            if (canMove)
            {
                Move(direction);
            }

            return canMove;
        }
        private void Move(Vector2 direction)
        {
            transform.position += new Vector3(direction.x * _moveAmount, 0, direction.y * _moveAmount);
            
            OnMoved();
        }
        
        private void OnMoved()
        {
            _rectangularArea.RectangularArea.UpdateState();
        }
    }
}