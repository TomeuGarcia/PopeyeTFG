using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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

        [System.Serializable]
        public class SubPoints
        {
            [SerializeField] private int _stemmingFromIndex = 0;
            
            public Vector3[] points = { 
                Vector3.left * 2, 
                Vector3.right * 2, 
                Vector3.right * 2 + Vector3.forward * 3 
            };

            public int StemmingFromIndex => _stemmingFromIndex;

            public void ApplyCorrection(int maxIndex)
            {
                _stemmingFromIndex = Mathf.Clamp(_stemmingFromIndex, 0, maxIndex);

                for (int i = 1; i < points.Length; ++i)
                {
                    if (points[i - 1] == points[i])
                    {
                        points[i] += Vector3.right;
                    }
                }
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

        [Header("SUB-POINTS")]
        [SerializeField] private SubPoints[] _subPointsList;
        public SubPoints[] SubPointsList => _subPointsList;
        
        
        public Vector3[] Points => _points;
        private Block CornerBlock => _config.CornerBlock;
        private Block FillBlock => _config.FillBlock;
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

            foreach (SubPoints subPoints in _subPointsList)
            {
                subPoints.ApplyCorrection(_points.Length - 1);
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

            if (_subPointsList.Length > 0)
            {
                foreach (SubPoints subPoints in _subPointsList)
                {
                    if (subPoints.points.Length == 0) continue;
                    FillSubPointsWall(subPoints);
                }
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
                Vector3 point = PointToWorldSpace(_points[i]);
                CreateCornerWall(point);
            }
            
            if (FillBlock.Length < 0.01f)
            {
                return;
            }
            
            CreateColliderForFirst();
            for (int i = 1; i < _points.Length; ++i)
            {
                Vector3 previousPointLocal = _points[i - 1];
                Vector3 previousPoint = PointToWorldSpace(previousPointLocal);
                Vector3 currentPoint = PointToWorldSpace(_points[i]);
                
                CreatFillWallsBetweenPoints(previousPointLocal, previousPoint, currentPoint);
            }

        }

        private void FillSubPointsWall(SubPoints subPoints)
        {
            Vector3[] points = subPoints.points;
            
            for (int i = 0; i < points.Length; ++i)
            {
                Vector3 point = PointToWorldSpace(points[i]);
                CreateCornerWall(point);
            }
            
            if (FillBlock.Length < 0.01f)
            {
                return;
            }
            
            
            Vector3 previousPointLocal = _points[subPoints.StemmingFromIndex];
            Vector3 previousPoint = PointToWorldSpace(previousPointLocal);
            Vector3 currentPoint = PointToWorldSpace(points[0]);
                
            CreatFillWallsBetweenPoints(previousPointLocal, previousPoint, currentPoint);
            
            for (int i = 1; i < points.Length; ++i)
            {
                previousPointLocal = points[i - 1];
                previousPoint = PointToWorldSpace(previousPointLocal);
                currentPoint = PointToWorldSpace(points[i]);
                
                CreatFillWallsBetweenPoints(previousPointLocal, previousPoint, currentPoint);
            }
        }


        private void CreatFillWallsBetweenPoints(Vector3 previousPointLocal, Vector3 previousPoint, Vector3 currentPoint)
        {
            Vector3 previousToCurrent = currentPoint - previousPoint;
            float previousToCurrentDistance = previousToCurrent.magnitude;
            Vector3 previousToCurrentDirection = previousToCurrent / previousToCurrentDistance;
                
            Quaternion offsetRotation = Quaternion.LookRotation(previousToCurrentDirection, Vector3.up);
                
            float fillLength = previousToCurrentDistance - CornerBlock.Length;

                
            CreateFillWalls(previousPoint, previousToCurrentDirection, previousToCurrentDistance, offsetRotation,
                fillLength);
                
            CreateColliderPreviousToCurrent(previousPointLocal, previousToCurrentDirection, previousToCurrentDistance, offsetRotation,
                fillLength);
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
        

        private void CreateColliderPreviousToCurrent(Vector3 previousPointLocal, Vector3 previousToCurrentDirection, float previousToCurrentDistance,  
            Quaternion rotation, float fillLength)
        {
            Quaternion offsetRotation = transform.rotation;
            Quaternion revertRotation =  Quaternion.Inverse(offsetRotation);
            
            Vector3 startPoint = offsetRotation * previousPointLocal;
            
            Vector3 colliderPosition =  startPoint + (previousToCurrentDirection * ((fillLength / 2) + CornerBlock.Length));
            Vector3 colliderSize = rotation * 
                                   (new Vector3(_config.ColliderWidth, _config.ColliderHeight, previousToCurrentDistance));

            colliderPosition = revertRotation * colliderPosition;
            colliderSize = revertRotation * colliderSize;
            
            DoCreateCollider(colliderPosition, colliderSize);
        }
        
        private void CreateColliderForFirst()
        {
            Vector3 colliderPosition = _points[0];
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

            MovePointsRangeByAmount(_points, offset, 0, _points.Length);

            foreach (SubPoints subPoints in _subPointsList)
            {
                MovePointsRangeByAmount(subPoints.points, offset, 0, subPoints.points.Length);
            }
        }

        public void MoveBasePointsRangeByAmount(Vector2 moveAmount, int startInclusive, int endExclusive)
        {
            MovePointsRangeByAmount(_points, moveAmount, startInclusive, endExclusive);
        }
        
        public void MovePointsRangeByAmount(Vector3[] points, Vector2 moveAmount, int startInclusive, int endExclusive)
        {
            for (int i = startInclusive; i < endExclusive; ++i)
            {
                points[i] -= new Vector3(moveAmount.x, 0, moveAmount.y);
            }
        }



        private Vector3[] GetCornersOfPointIndices(int previousIndex, int currentIndex)
        {
            Vector3 previousPoint = PointToWorldSpace(_points[previousIndex]);
            Vector3 currentPoint = PointToWorldSpace(_points[currentIndex]);

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
        private float _colorMultiplier;
        
        private void OnDrawGizmos()
        {
            _colorMultiplier = 1.0f;
            Draw(_points);

            _colorMultiplier = EditorView.StemmingBlocksColorMultiplier;
            foreach (SubPoints subPoints in _subPointsList)
            {
                if (subPoints.points.Length == 0) continue;
                
                DrawFillBlocks(
                    PointToWorldSpace(_points[subPoints.StemmingFromIndex]), 
                    PointToWorldSpace(subPoints.points[0])
                    );

                Draw(subPoints.points);
            }
        }
        
        private void Draw(Vector3[] points)
        {
            Vector3[] drawSpacePoints = new Vector3[points.Length];
            
            
            for (int i = 0; i < points.Length; ++i)
            {
                drawSpacePoints[i] = PointToWorldSpace(points[i]);
                Handles.color = _config.TransparencyConfig
                    .ApplyTransparencyToColor(_config.EditorView.CornerBlockColor * _colorMultiplier, Position);
                
                DrawBlock(CornerBlock, drawSpacePoints[i], Quaternion.identity);
            }


            if (FillBlock.Length < 0.01f)
            {
                return;
            }
            
            for (int i = 1; i < points.Length; ++i)
            {
                Vector3 previousPoint = drawSpacePoints[i - 1];
                Vector3 currentPoint = drawSpacePoints[i];

                DrawFillBlocks(previousPoint, currentPoint);
            }
        }

        private void DrawFillBlocks(Vector3 previousPoint, Vector3 currentPoint)
        {
            Handles.color = _config.TransparencyConfig
                .ApplyTransparencyToColor(_config.EditorView.FillLineColor * _colorMultiplier, Position);
            Handles.DrawLine(previousPoint, currentPoint, _config.EditorView.LineThickness);
            

            Vector3 previousToCurrent = currentPoint - previousPoint;
            float previousToCurrentDistance = previousToCurrent.magnitude;
            Vector3 previousToCurrentDirection = previousToCurrent / previousToCurrentDistance;
                
            Quaternion offsetRotation = Quaternion.LookRotation(previousToCurrentDirection, Vector3.up);
                
            Handles.color = _config.TransparencyConfig
                .ApplyTransparencyToColor(_config.EditorView.FillBlockColor * _colorMultiplier, Position);

            float lineLength = previousToCurrentDistance - CornerBlock.Length;
            float distanceCounter = CornerBlock.Length / 2 + FillBlock.Length / 2;

            for (; distanceCounter < lineLength; distanceCounter += FillBlock.Length)
            {
                Vector3 fillPosition = previousPoint + (previousToCurrentDirection * distanceCounter);
                DrawBlock(FillBlock, fillPosition, offsetRotation);
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

        private Vector3 PointToWorldSpace(Vector3 point)
        {
            return transform.TransformPoint(point);
        }
#endif
        
    }
    
}