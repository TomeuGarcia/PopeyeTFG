namespace Project.Scripts.Math.Functions
{
    public class Segment
    {
        public float X { get; private set; } // x
        public float Y { get; private set; } // f(x)
        public float M { get; private set; } // f'(x) (Slope)

        public Segment(float x, float y, float m)
        {
            X = x;
            Y = y;
            M = m;
        }
    }
}