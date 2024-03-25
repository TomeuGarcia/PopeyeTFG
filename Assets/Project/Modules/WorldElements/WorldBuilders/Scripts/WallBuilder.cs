using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Popeye.Core.Pool;
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
                    
                    currentPoint.SubPointsGroup.ApplyCorrections();
                    previousPoint = currentPoint;
                }
            }

            public void ApplyOffset(Vector3 offset)
            {
                foreach (CornerPoint cornerPoint in CornerPoints)
                {
                    cornerPoint.Position += offset;
                    cornerPoint.SubPointsGroup.ApplyOffset(offset);
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

            public void AddPoint()
            {
                bool isFistPoint = NumberOfCornerPoints < 1;
                Vector3 position = isFistPoint ? CORRECTION_OFFSET : _cornerPoints[^1] + CORRECTION_OFFSET;
                CornerPoints.Add(new CornerPoint(position));
            }
        }

        [System.Serializable]
        public class CornerPoint
        {
            [ProgressBar("isSelected", 1, EColor.Green)]
            public int isSelectedValue = 1;
            
            [SerializeField] private Vector3 _position;
            
            [SerializeField, Range(-10, 10)] private int _extrudeTimes = 0;
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

            public int ExtrudeTimes => _extrudeTimes;

            public CornerPoint(Vector3 position)
            {
                _position = position;
                _subPointsGroup = new CornerPointsGroup();
            }
            
            public static implicit operator Vector3(CornerPoint cornerPoint)
            {
                return cornerPoint.Position;
            }

            public void SetSelected(bool isSelected)
            {
                isSelectedValue = isSelected ? 1 : 0;
            }
        }

        [Header("PARENTS")] 
        [SerializeField] private Transform _cornerWallsParent;
        [SerializeField] private Transform _fillWallsParent;
        [SerializeField] private GameObject _collidersParent;
        
        [Header("CONFIG")]
        [Expandable] [SerializeField] private WallBuilderConfig _config;
        
        [Header("POINTS")]
        [SerializeField] private CornerPointsGroup _baseCornerPointsGroup;
        public CornerPointsGroup BaseCornerPointsGroup => _baseCornerPointsGroup;

        
        [SerializeField] private SerializableObjectBuffer _cornersObjectBuffer;
        [SerializeField] private SerializableObjectBuffer _fillsObjectBuffer;

        private Block CornerBlock => _config.CornerBlock;
        private Block FillBlock => _config.FillBlock;
        public WallBuilderConfig.EditorViewConfig EditorView => _config.EditorView;
        

        private Vector3 Position => transform.position;


        private void OnValidate()
        {
            if (IsReadyForEditor())
            {
                CornerBlock.UpdateHalfSize();
                FillBlock.UpdateHalfSize();
            }
            
            _baseCornerPointsGroup.ApplyCorrections();
            
            if (!_cornerWallsParent) _cornerWallsParent = transform;
            if (!_fillWallsParent) _fillWallsParent = transform;
            if (!_collidersParent) _collidersParent = gameObject;
        }
        


        private void SpawnWalls()
        {
            if (_baseCornerPointsGroup.NumberOfCornerPoints < 1)
            {
                return;
            }


            while (_collidersParent.TryGetComponent<Collider>(out Collider collider))
            {
                DestroyImmediate(collider);
            }

            _cornersObjectBuffer.SetupBeforeUse();
            _fillsObjectBuffer.SetupBeforeUse();

            FillWallsNew(_baseCornerPointsGroup);
            CreateFakeMesh();
            
            _cornersObjectBuffer.ClearAfterUse();
            _fillsObjectBuffer.ClearAfterUse();
        }
        

        public bool IsReadyForEditor()
        {
            return _config != null && _baseCornerPointsGroup.NumberOfCornerPoints > 1;
        }


        private void FillWallsNew(CornerPointsGroup cornerPointsGroup)
        {
            if (cornerPointsGroup.NumberOfCornerPoints < 1) return;
            
            
            List<CornerPoint> cornerPoints = cornerPointsGroup.CornerPoints;
            
            for (int i = 0; i < cornerPointsGroup.NumberOfCornerPoints; ++i)
            {
                CornerPoint cornerPoint = cornerPoints[i];
                Vector3 pointLocal = cornerPoint.Position;
                Vector3 point = PointToWorldSpace(pointLocal);
                CreateCornerWall(point);
                ExtrudeCornerWall(point, cornerPoint);
                
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
            CreateWall(_config.CornerBlockPrefab, position, Quaternion.identity, _cornersObjectBuffer);
        }

        private void ExtrudeCornerWall(Vector3 position, CornerPoint cornerPoint)
        {
            if (cornerPoint.ExtrudeTimes == 0)
            {
                return;
            }
            
            int extrudeTimes = Mathf.Abs(cornerPoint.ExtrudeTimes);
            Vector3 extrudeOffset = cornerPoint.ExtrudeTimes > 0 
                ? _config.ExtrudePositiveOffset 
                : _config.ExtrudeNegativeOffset;
            
            
            for (int extrudeI = 1; extrudeI <= extrudeTimes; ++extrudeI)
            {
                CreateCornerWall(position + (extrudeOffset * extrudeI));
            }


            Vector3 startPosition = PointToWorldSpace(position);
            Vector3 endPosition = PointToWorldSpace(position + (extrudeOffset * extrudeTimes));

            Vector3 colliderPosition = Vector3.Lerp(startPosition, endPosition, 0.5f);

            Vector3 colliderScaler = _config.ExtrudePositiveOffset * (extrudeTimes/2f);
            
            Vector3 colliderSize = new Vector3(_config.ColliderWidth, _config.ColliderHeight, CornerBlock.Length);
            colliderSize.x *= Mathf.Max(colliderScaler.x, 1);
            colliderSize.y *= Mathf.Max(colliderScaler.y, 1);
            colliderSize.z *= Mathf.Max(colliderScaler.z, 1);

            DoCreateCollider(colliderPosition, colliderSize);
        }
        

        private void CreateFillWalls(Vector3 previousPoint, Vector3 previousToCurrentDirection, float previousToCurrentDistance,
            Quaternion rotation, float fillLength)
        {
            float distanceCounter = CornerBlock.Length / 2 + FillBlock.Length / 2;
            
            for (; distanceCounter < fillLength; distanceCounter += FillBlock.Length)
            {
                Vector3 fillPosition = previousPoint + (previousToCurrentDirection * distanceCounter);
                CreateFillWall(fillPosition, rotation);
            }
        }
        
        private void CreateFillWall(Vector3 position, Quaternion rotation)
        {
            CreateWall(_config.FillBlockPrefab, position, rotation, _fillsObjectBuffer);
        }

        private void CreateWall(GameObject wallPrefab, Vector3 position, Quaternion rotation, 
            SerializableObjectBuffer objectBuffer)
        {
            if (objectBuffer.HasObjectsLeft(out GameObject bufferObject))
            {
                bufferObject.transform.position = position;
                bufferObject.transform.rotation = rotation;
                bufferObject.transform.parent = _fillWallsParent;
            }
            else
            {
                bufferObject = Instantiate(wallPrefab, position, rotation, _fillWallsParent);
            }
            
            objectBuffer.AddToSpawnedObjectsBuffer(bufferObject);
            
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
            Vector3 colliderPosition = _baseCornerPointsGroup.CornerPoints[0].Position;
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
            if (!_collidersParent.TryGetComponent<MeshFilter>(out MeshFilter meshFilter))
            {
                _collidersParent.AddComponent<MeshFilter>();
                Debug.Log("no mesh filter");
            }

            if (!_collidersParent.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
            {
                meshRenderer = _collidersParent.AddComponent<MeshRenderer>();
                Debug.Log("no mesh renderer");
            }
            
            meshRenderer.material = _config.FakeMeshMaterial;
            meshRenderer.enabled = false;
        }


        public void AddPoint()
        {
            _baseCornerPointsGroup.AddPoint();
        }
        
        public void CenterAroundPivot(out Vector2 offset)
        {
            Vector2 minimums = Vector2.one * float.MaxValue;
            Vector2 maximums = Vector2.one * float.MinValue;
            
            foreach (CornerPoint baseCornerPoint in _baseCornerPointsGroup.CornerPoints)
            {
                Vector3 point = baseCornerPoint.Position;

                if (point.x < minimums.x) minimums.x = point.x;
                if (point.z < minimums.y) minimums.y = point.z;
                
                if (point.x > maximums.x) maximums.x = point.x;
                if (point.z > maximums.y) maximums.y = point.z;
            }

            
            offset = (minimums + maximums) / 2;
            Vector3 offset3D = new Vector3(offset.x, 0, offset.y);
            _baseCornerPointsGroup.ApplyOffset(offset3D);
        }

        public void MovePivotToCenter()
        {
            CenterAroundPivot(out Vector2 offset);

            transform.position += new Vector3(offset.x, 0, offset.y);
        }
        

        private Vector3 PointToWorldSpace(Vector3 point)
        {
            return transform.TransformPoint(point);
        }

        public void BakeWalls()
        {
            OnValidate();
            SpawnWalls();
        }
        
        public void ClearBakedWalls()
        {
            OnValidate();
            
            _cornersObjectBuffer.DestroyObjectsAndClearBuffer();
            _fillsObjectBuffer.DestroyObjectsAndClearBuffer();

            while (_collidersParent.TryGetComponent<Collider>(out Collider collider))
            {
                DestroyImmediate(collider);
            }
        }
        
#if UNITY_EDITOR
        private float _colorMultiplier;
        
        private void OnDrawGizmos()
        {
            if (!IsReadyForEditor()) return;
            
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
                CornerPoint cornerPoint = pointsGroup.CornerPoints[i];
                Vector3 drawSpacePoint =  PointToWorldSpace(cornerPoint.Position);
                
                Handles.color = _config.TransparencyConfig.ApplyTransparencyToColor(color, Position);
                DrawBlock(CornerBlock, drawSpacePoint, Quaternion.identity);

                if (cornerPoint.ExtrudeTimes != 0)
                {
                    bool positiveExtrude = cornerPoint.ExtrudeTimes > 0;
                    Vector3 pointExtrude = drawSpacePoint + _config.ExtrudePositiveOffset * 
                        (cornerPoint.ExtrudeTimes + (positiveExtrude ? 1 : -1));
                    DrawBlock(CornerBlock, pointExtrude, Quaternion.identity);
                }

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
            if (Handles.color.a < 0.001f) return;
            
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