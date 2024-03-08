using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Popeye.Modules.WorldElements.WorldBuilders
{
    public class WallBuilder : MonoBehaviour
    {
        [System.Serializable]
        public class Block
        {
            [SerializeField] private Vector2 _size = Vector2.one;
            private Vector2 _halfSize;
            public Vector2 Size => _size;
            public Vector3 WorldSpaceSize =>  new Vector3(_size.x, 0, _size.y);
            public float Length => Size.y;

            public Vector3[] ToFrame(Vector3 center, Quaternion rotation)
            {
                Vector3[] framePositions = new Vector3[4];

                framePositions[0] = center + (rotation * (Vector3.right * _halfSize.x +   Vector3.forward * _halfSize.y));
                framePositions[1] = center + (rotation * (Vector3.right * _halfSize.x +   Vector3.back * _halfSize.y));
                framePositions[2] = center + (rotation * (Vector3.left * _halfSize.x +    Vector3.back * _halfSize.y));
                framePositions[3] = center + (rotation * (Vector3.left * _halfSize.x +    Vector3.forward * _halfSize.y));

                return framePositions;
            }

            public void UpdateHalfSize()
            {
                _halfSize = Size / 2;
            }
        }

        [Header("PARENTS")] 
        [SerializeField] private Transform _cornerWallsParent;
        [SerializeField] private Transform _fillWallsParent;
        [SerializeField] private GameObject _collidersParent;
        
        [Header("CONFIG")]
        [Expandable] [SerializeField] private WallBuilderConfig _config;
        
        [Header("POINTS")]
        [SerializeField] private Vector3[] _points = { 
            Vector3.left * 2, 
            Vector3.right * 2, 
            Vector3.right * 2 + Vector3.forward * 3 
        };

        public Vector3[] Points => _points;
        public Block CornerBlock => _config.CornerBlock;
        public Block FillBlock => _config.FillBlock;
        public WallBuilderConfig.EditorViewConfig EditorView => _config.EditorView;

        [HideInInspector] public Vector2 moveBy = Vector2.zero;
        [HideInInspector] public Vector2Int selectedPointsRange = Vector2Int.zero;

        private Vector3 Position => transform.position;


        private void OnValidate()
        {
            if (IsReadyForEditor())
            {
                CornerBlock.UpdateHalfSize();
                FillBlock.UpdateHalfSize();
            }
        }

        private void Awake()
        {
            OnValidate();

            if (!_cornerWallsParent) _cornerWallsParent = transform;
            if (!_fillWallsParent) _fillWallsParent = transform;
            if (!_collidersParent) _collidersParent = gameObject;
        }

        private void Start()
        {
            if (_points.Length > 0)
            {
                FillWalls();
                CreateFakeMesh();
            }
        }
        
        public void AddPoint()
        {
            Array.Resize(ref _points, _points.Length + 1);

            if (_points.Length > 1)
            {
                _points[^1] = _points[^2] + Vector3.right;    
            }
            else
            {
                _points[0] = Vector3.right;
            }
        }

        public bool IsReadyForEditor()
        {
            return _config != null && _points.Length > 1;
        }


        private void FillWalls()
        {
            for (int i = 0; i < _points.Length; ++i)
            {
                Vector3 point = transform.TransformPoint(_points[i]);
                CreateCornerWall(point);
            }
            
            if (FillBlock.Length < 0.01f)
            {
                return;
            }
            
            for (int i = 1; i < _points.Length; ++i)
            {
                Vector3 previousPoint = transform.TransformPoint(_points[i - 1]);
                Vector3 currentPoint = transform.TransformPoint(_points[i]);
                

                Vector3 previousToCurrent = currentPoint - previousPoint;
                float previousToCurrentDistance = previousToCurrent.magnitude;
                Vector3 previousToCurrentDirection = previousToCurrent / previousToCurrentDistance;
                
                Quaternion offsetRotation = Quaternion.LookRotation(previousToCurrentDirection, Vector3.up);
                
                float fillLength = previousToCurrentDistance - CornerBlock.Length;

                
                CreateFillWalls(previousPoint, previousToCurrentDirection, previousToCurrentDistance, offsetRotation,
                    fillLength);
                
                CreateColliderPreviousToCurrent(i, previousToCurrentDirection, previousToCurrentDistance, offsetRotation,
                    fillLength);
                    
            }

            CreateColliderForLast();
        }


        private void CreateCornerWall(Vector3 position)
        {
            InstantiateWall(_config.CornerBlockPrefab, position, Quaternion.identity);
        }

        private void CreateFillWalls(Vector3 previousPoint, Vector3 previousToCurrentDirection, float previousToCurrentDistance,
            Quaternion rotation, float fillLength)
        {
            float distanceCounter = CornerBlock.Length / 2 + FillBlock.Length / 2;
            
            for (; distanceCounter < fillLength; distanceCounter += FillBlock.Length)
            {
                Vector3 fillPosition = previousPoint + (previousToCurrentDirection * distanceCounter);
                InstantiateWall(_config.FillBlockPrefab, fillPosition, rotation);
            }
        }

        private void InstantiateWall(GameObject wallPrefab, Vector3 position, Quaternion rotation)
        {
            Instantiate(wallPrefab, position, rotation, _fillWallsParent);
            _config.WallBuilderDataTracker.OnWallInstantiated();
        }
        

        private void CreateColliderPreviousToCurrent(int index, Vector3 previousToCurrentDirection, float previousToCurrentDistance,  
            Quaternion rotation, float fillLength)
        {
            Vector3 colliderPosition = _points[index - 1] + (previousToCurrentDirection * (fillLength / 2));
            Vector3 colliderSize = rotation * 
                                   (new Vector3(_config.ColliderWidth, _config.ColliderHeight, previousToCurrentDistance)); 
                
            DoCreateCollider(colliderPosition, colliderSize);
        }
        
        private void CreateColliderForLast()
        {
            Vector3 colliderPosition = _points[^1];
            Vector3 colliderSize = new Vector3(_config.ColliderWidth, _config.ColliderHeight, CornerBlock.Length);

            DoCreateCollider(colliderPosition, colliderSize);
        }

        private void DoCreateCollider(Vector3 colliderPosition, Vector3 colliderSize)
        {
            colliderSize.x = Mathf.Abs(colliderSize.x);
            colliderSize.y = Mathf.Abs(colliderSize.y);
            colliderSize.z = Mathf.Abs(colliderSize.z);
            
            BoxCollider boxCollider = _collidersParent.AddComponent<BoxCollider>();
            boxCollider.center = colliderPosition + (Vector3.up * _config.HalfColliderHeight);
            boxCollider.size = colliderSize;
        }



        private void CreateFakeMesh()
        {
            _collidersParent.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = _collidersParent.AddComponent<MeshRenderer>();
            meshRenderer.material = _config.FakeMeshMaterial;
            meshRenderer.enabled = false;
        }


        public void CenterAroundPivot()
        {
            Vector2 minimums = Vector2.one * float.MaxValue;
            Vector2 maximums = Vector2.one * float.MinValue;
            
            foreach (Vector3 point in _points)
            {
                if (point.x < minimums.x) minimums.x = point.x;
                if (point.z < minimums.y) minimums.y = point.z;
                
                if (point.x > maximums.x) maximums.x = point.x;
                if (point.z > maximums.y) maximums.y = point.z;
            }

            
            Vector2 offset = (minimums + maximums) / 2;

            MovePointsRangeByAmount(offset, 0, _points.Length);
        }

        public void MovePointsRangeByAmount(Vector2 moveAmount, int startInclusive, int endExclusive)
        {
            for (int i = startInclusive; i < endExclusive; ++i)
            {
                _points[i] -= new Vector3(moveAmount.x, 0, moveAmount.y);
            }
        }



        private Vector3[] GetCornersOfPointIndices(int previousIndex, int currentIndex)
        {
            Vector3 previousPoint = transform.TransformPoint(_points[previousIndex]);
            Vector3 currentPoint = transform.TransformPoint(_points[currentIndex]);

            Vector3 previousToCurrentDirection = (currentPoint - previousPoint).normalized;
            Vector3 sideDirection = Vector3.Cross(previousToCurrentDirection, Vector3.up).normalized;

            Vector3 cornerBlockSize = CornerBlock.WorldSpaceSize;

            Vector3 forwardProjectedSize = Vector3.Project(cornerBlockSize, sideDirection) / 2;
            Vector3 sideProjectedSize = Vector3.Project(cornerBlockSize, previousToCurrentDirection) / 2;
            
            Vector3 previousCornerA = previousPoint - forwardProjectedSize - sideProjectedSize;
            Vector3 previousCornerB = previousPoint + forwardProjectedSize + sideProjectedSize;
            Vector3 currentCornerA = currentPoint - forwardProjectedSize - sideProjectedSize;
            Vector3 currentCornerB = currentPoint + forwardProjectedSize + sideProjectedSize;

            Vector3[] corners =
            {
                previousCornerA, previousCornerB, currentCornerB, currentCornerA
            };

            return corners;
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Draw();
        }
        
        private void Draw()
        {
            Vector3[] drawSpacePoints = new Vector3[_points.Length];
            
            
            for (int i = 0; i < _points.Length; ++i)
            {
                drawSpacePoints[i] = transform.TransformPoint(_points[i]);
                Handles.color = _config.TransparencyConfig.ApplyTransparencyToColor(_config.EditorView.CornerBlockColor, Position);
                DrawBlock(CornerBlock, drawSpacePoints[i], Quaternion.identity);
            }


            for (int i = 1; i < _points.Length; ++i)
            {
                Vector3 previousPoint = drawSpacePoints[i - 1];
                Vector3 currentPoint = drawSpacePoints[i];

                Handles.color = _config.TransparencyConfig.ApplyTransparencyToColor(_config.EditorView.FillLineColor, Position);
                Handles.DrawLine(previousPoint, currentPoint, _config.EditorView.LineThickness);

                if (FillBlock.Length < 0.01f)
                {
                    continue;
                }

                Vector3 previousToCurrent = currentPoint - previousPoint;
                float previousToCurrentDistance = previousToCurrent.magnitude;
                Vector3 previousToCurrentDirection = previousToCurrent / previousToCurrentDistance;
                
                Quaternion offsetRotation = Quaternion.LookRotation(previousToCurrentDirection, Vector3.up);
                
                Handles.color = _config.TransparencyConfig.ApplyTransparencyToColor(_config.EditorView.FillBlockColor, Position);

                float lineLength = previousToCurrentDistance - CornerBlock.Length;
                float distanceCounter = CornerBlock.Length / 2 + FillBlock.Length / 2;

                for (; distanceCounter < lineLength; distanceCounter += FillBlock.Length)
                {
                    Vector3 fillPosition = previousPoint + (previousToCurrentDirection * distanceCounter);
                    DrawBlock(FillBlock, fillPosition, offsetRotation);
                }
            }
        }
        
        private void DrawBlock(Block block, Vector3 center, Quaternion rotation)
        {
            Vector3[] framePoints = block.ToFrame(center, rotation);
            for (int i = 0; i < framePoints.Length; ++i)
            {
                Handles.DrawLine(framePoints[i], framePoints[(i + 1) % framePoints.Length], _config.EditorView.LineThickness);
            }

            Vector3 lookA = Vector3.Lerp(framePoints[0], framePoints[1], 0.5f);
            Vector3 lookB = Vector3.Lerp(framePoints[2], framePoints[3], 0.5f);
            Vector3 lookCenter = Vector3.Lerp(framePoints[0], framePoints[3], 0.5f);
            
            Handles.DrawLine(lookA, lookCenter);
            Handles.DrawLine(lookB, lookCenter);
        }
#endif
        
    }
    
}