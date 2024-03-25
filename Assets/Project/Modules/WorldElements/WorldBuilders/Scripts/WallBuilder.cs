using System;
using System.Collections.Generic;
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

        [System.Serializable]
        public class CornerPointsGroup
        {
            [SerializeField] private List<CornerPoint> _cornerPoints;
            public List<CornerPoint> CornerPoints => _cornerPoints;
            public int NumberOfCornerPoints => _cornerPoints.Count;

            public CornerPointsGroup()  : this(3) { }
            public CornerPointsGroup(int capacity)
            {
                _cornerPoints = new List<CornerPoint>(capacity);
            }
            public CornerPointsGroup(Vector3[] points)
            {
                _cornerPoints = new List<CornerPoint>(points.Length);
                for (int i = 0; i < points.Length; ++i)
                {
                    _cornerPoints.Add(new CornerPoint(points[i]));
                }
            }

            private static readonly Vector3 CORRECTION_OFFSET = Vector3.right;
            public void ApplyCorrections()
            {
                if (_cornerPoints.Count < 1)
                {
                    return;
                }
                
                CornerPoint previousPoint = _cornerPoints[0];
                previousPoint.SubPointsGroup.ApplyCorrections();
                
                for (int i = 1; i < _cornerPoints.Count; ++i)
                {
                    CornerPoint currentPoint = _cornerPoints[i];
                    if (previousPoint.Position == currentPoint.Position)
                    {
                        currentPoint.Position += CORRECTION_OFFSET;
                    }
                    previousPoint = currentPoint;

                    currentPoint.SubPointsGroup.ApplyCorrections();
                }
            }

            public int GetDepth()
            {
                int depthCount = 0;

                if (NumberOfCornerPoints < 1) return 0;
                
                int[] depths = new int[NumberOfCornerPoints];
                
                for (int i1 = 0; i1 < NumberOfCornerPoints; ++i1)
                {
                    depths[i1] = _cornerPoints[i1].SubPointsGroup.GetDepth() + 1;
                }

                return Mathf.Max(depths);
            }
        }

        [System.Serializable]
        public class CornerPoint
        {
            [SerializeField] private Vector3 _position;
            [SerializeField] private CornerPointsGroup _subPointsGroup;
            
            public Vector3 Position
            {
                get => _position;
                set => _position = value;
            }

            public CornerPointsGroup SubPointsGroup 
            {
                get => _subPointsGroup;
                set => _subPointsGroup = value;
            }

            public CornerPoint(Vector3 position)
            {
                _position = position;
                _subPointsGroup = new CornerPointsGroup();
            }
            
            public static implicit operator Vector3(CornerPoint cornerPoint)
            {
                return cornerPoint.Position;
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

        [SerializeField] private CornerPointsGroup _baseCornerPointsGroup;
        public CornerPointsGroup BaseCornerPointsGroup => _baseCornerPointsGroup;
        

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
            
            _baseCornerPointsGroup.ApplyCorrections();
            
            //CopyPointsToCornerPoints();
        }
        
        public void CopyPointsToCornerPoints()
        {
            _baseCornerPointsGroup = new CornerPointsGroup(_points);
    
            foreach (SubPoints subPoints in _subPointsList)
            {
                _baseCornerPointsGroup.CornerPoints[subPoints.StemmingFromIndex].SubPointsGroup =
                    new CornerPointsGroup(subPoints.points);
            }
            
            Debug.Log("Corners copied: " + gameObject.name);
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
            if (_config.MetaConfig.DestroyOnPlay)
            {
                Destroy(this);
                return;
            }

            if (_baseCornerPointsGroup.NumberOfCornerPoints > 0)
            {
                FillWallsNew(_baseCornerPointsGroup);
                CreateFakeMesh();
            }
            
        }
        

        public bool IsReadyForEditor()
        {
            return _config != null && _points.Length > 1;
        }


        private void FillWallsNew(CornerPointsGroup cornerPointsGroup)
        {
            if (cornerPointsGroup.NumberOfCornerPoints < 1) return;
            
            
            List<CornerPoint> cornerPoints = cornerPointsGroup.CornerPoints;
            
            for (int i = 0; i < cornerPointsGroup.NumberOfCornerPoints; ++i)
            {
                Vector3 pointLocal = cornerPoints[i].Position;
                Vector3 point = PointToWorldSpace(pointLocal);
                CreateCornerWall(point);

                CornerPointsGroup subPointsGroup = cornerPoints[i].SubPointsGroup;
                if (subPointsGroup.NumberOfCornerPoints > 0)
                {
                    Vector3 currentPoint = PointToWorldSpace(subPointsGroup.CornerPoints[0].Position);
                    CreatFillWallsBetweenPoints(pointLocal, point, currentPoint);
                    FillWallsNew(subPointsGroup);
                }
            }
            
            if (FillBlock.Length < 0.01f)
            {
                return;
            }
            
            CreateColliderForFirst();
            for (int i = 1; i < cornerPointsGroup.NumberOfCornerPoints; ++i)
            {
                Vector3 previousPointLocal = cornerPoints[i - 1].Position;
                Vector3 previousPoint = PointToWorldSpace(previousPointLocal);
                Vector3 currentPoint = PointToWorldSpace(cornerPoints[i].Position);
                
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


        public void CenterAroundPivot(out Vector2 offset)
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

            
            offset = (minimums + maximums) / 2;

            MovePointsRangeByAmount(_points, offset, 0, _points.Length);

            foreach (SubPoints subPoints in _subPointsList)
            {
                MovePointsRangeByAmount(subPoints.points, offset, 0, subPoints.points.Length);
            }
        }

        public void MovePivotToCenter()
        {
            CenterAroundPivot(out Vector2 offset);

            transform.position += new Vector3(offset.x, 0, offset.y);
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

        private Vector3 PointToWorldSpace(Vector3 point)
        {
            return transform.TransformPoint(point);
        }


#if UNITY_EDITOR
        private float _colorMultiplier;
        
        private void OnDrawGizmos()
        {
            int baseDepth = _baseCornerPointsGroup.GetDepth();
            DrawCorners(_baseCornerPointsGroup, 0, baseDepth);
            DrawFills(_baseCornerPointsGroup, _baseCornerPointsGroup.CornerPoints[0], 0, baseDepth);
        }
        
        
        private void DrawCorners(CornerPointsGroup pointsGroup, int depth, int baseDepth)
        {
            float colorMultiplier = 1f - ((float)(depth) / baseDepth);
            Color color = _config.EditorView.CornerBlockColor * colorMultiplier;
            color.a = 1f;

            for (int i = 0; i < pointsGroup.NumberOfCornerPoints; ++i)
            {
                Vector3 drawSpacePoint =  PointToWorldSpace(pointsGroup.CornerPoints[i].Position);
                
                Handles.color = _config.TransparencyConfig.ApplyTransparencyToColor(color, Position);
                DrawBlock(CornerBlock, drawSpacePoint, Quaternion.identity);
                
                DrawCorners(pointsGroup.CornerPoints[i].SubPointsGroup, depth + 1, baseDepth);
            }
        }
        
        private void DrawFills(CornerPointsGroup pointsGroup, CornerPoint startPoint, int depth, int baseDepth)
        {
            float colorMultiplier = 1f - ((float)(depth) / baseDepth);
            Color color = _config.EditorView.FillBlockColor * colorMultiplier;
            color.a = 1f;
            

            Vector3 previousPoint = PointToWorldSpace(startPoint.Position);
            Vector3 currentPoint = Vector3.zero;
            
            for (int i = 0; i < pointsGroup.NumberOfCornerPoints; ++i)
            {
                Vector3 drawSpacePoint =  PointToWorldSpace(pointsGroup.CornerPoints[i].Position);
                
                
                currentPoint = drawSpacePoint;
                _colorMultiplier = colorMultiplier;
                DrawFillBlocks(previousPoint, currentPoint);
                previousPoint = currentPoint;
                
                DrawFills(pointsGroup.CornerPoints[i].SubPointsGroup, pointsGroup.CornerPoints[i],
                    depth + 1, baseDepth);
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

            Quaternion offsetRotation = Quaternion.identity;
            if (Vector3.Dot(previousToCurrentDirection, Vector3.up) < 0.99f)
            {
                offsetRotation = Quaternion.LookRotation(previousToCurrentDirection, Vector3.up);
            }            
                
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
        
#endif
        
    }
    
}