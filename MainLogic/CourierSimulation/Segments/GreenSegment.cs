namespace MainLogic.Segments;

public class GreenSegment : Segment
{
    public ContinuousGenerator Generator { get; }
    public GreenSegment(ContinuousGenerator generator, int distance) : base("green", distance)
    {
        Generator = generator;
    }

    public override double GetValue()
    {
        return Generator.Sample();
    }
}