using UnityEngine;

namespace Project.Modules.PlayerController.Testing.AutoAim.Scripts
{
    public class MathematicalFunction_Test
    {
        public class Segment
        {
            public float X { get; private set; }
            public float Y { get; private set; }
            public float M { get; private set; }

            public Segment(float x, float y, float m)
            {
                X = x;
                Y = y;
                M = m;
            }
        }
        
        public class Interval
        {
            private Segment _minSegment;
            private Segment _maxSegment;
            
            public Interval(Segment minSegment, Segment maxSegment)
            {
                _minSegment = minSegment;
                _maxSegment = maxSegment;
            }

            public bool Contains(float x)
            {
                return _minSegment.X < x && _maxSegment.X > x;
            }

            public float EvaluateCubicInterpolation(float x)
            {
                float delta = _maxSegment.X - _minSegment.X;
                float t = (x - _minSegment.X) / delta;
                
                CubicHermite(t, out float h00, out float h10, out float h01, out float h11);

                return (h00 * _minSegment.Y) +
                       (h10 * _minSegment.M * delta) +
                       (h01 * _maxSegment.Y) +
                       (h11 * _maxSegment.M * delta);
            }

            private void CubicHermite(float t, out float h00, out float h10, out float h01, out float h11)
            {
                float t2 = t * t;
                float t3 = t2 * t;

                float t2_2 = t2 * 2;
                float t2_3 = t2 * 3;
                float t3_2 = t3 * 2;

                h00 = t3_2 - t2_3 + 1;
                h10 = t3 - t2_2 + t;
                h01 = -t3_2 + t2_3;
                h11 = t3 - t2;
            }
        }   


        private Interval[] _intervals;
        
        public MathematicalFunction_Test(Vector2[] points)
        {
            // Interpolant selection
            float[] secantSlopes = new float[points.Length - 1];
            for (int i = 0; i < points.Length - 1; ++i)
            {
                secantSlopes[i] = (points[i + 1].y - points[i].y) / (points[i + 1].x - points[i].x);
            }
            
            float[] tangents = new float[points.Length];
            for (int i = 1; i < points.Length - 1; ++i)
            {
                tangents[i] = (secantSlopes[i - 1] + secantSlopes[i]) / 2;
            }

            tangents[0] = secantSlopes[0];
            tangents[^1] = secantSlopes[^1];


            for (int i = 0; i < points.Length - 1; ++i)
            {
                if (secantSlopes[i] == 0)
                {
                    tangents[i] = tangents[i + 1] = 0;
                    continue;
                }

                float alpha = tangents[i] / secantSlopes[i];
                float beta = tangents[i + 1] / secantSlopes[i];
                if (alpha < 0) tangents[i] = 0;
                if (beta < 0) tangents[i + 1] = 0;


                float alpha2 = alpha * alpha;
                float beta2 = beta * beta;
                if (alpha2 + beta2 > 9)
                {
                    float constraint = 3f / Mathf.Sqrt(alpha2 + beta2);
                    tangents[i] = constraint * alpha * secantSlopes[i];
                    tangents[i+1] = constraint * beta * secantSlopes[i];
                }
                
            }



            // Setup Segments
            Segment[] segments = new Segment[points.Length];
            for (int i = 0; i < points.Length; ++i)
            {
                segments[i] = new Segment(points[i].x, points[i].y, tangents[i]);
            }

            // Setup Intervals
            _intervals = new Interval[segments.Length - 1];
            for (int i = 0; i < segments.Length - 1; ++i)
            {
                _intervals[i] = new Interval(segments[i], segments[i + 1]);
            }
        }


        public float Evaluate(float x)
        {
            for (int i = 0; i < _intervals.Length - 1; ++i)
            {
                if (_intervals[i].Contains(x))
                {
                    return _intervals[i].EvaluateCubicInterpolation(x);
                }
            }
            
            return _intervals[^1].EvaluateCubicInterpolation(x);
        }
    }
    
    
}