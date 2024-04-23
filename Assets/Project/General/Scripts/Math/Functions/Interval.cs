namespace Project.Scripts.Math.Functions
{
    public class Interval
    {
        public readonly Segment MinSegment;
        public readonly Segment MaxSegment;
            
        public Interval(Segment minSegment, Segment maxSegment)
        {
            MinSegment = minSegment;
            MaxSegment = maxSegment;
        }

        public bool Contains(float x)
        {
            return MinSegment.X < x && MaxSegment.X > x;
        }
    }
}