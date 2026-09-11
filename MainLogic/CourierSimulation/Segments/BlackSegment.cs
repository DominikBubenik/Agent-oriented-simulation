namespace MainLogic.Segments;

public class BlackSegment : Segment
{
    public ContinuousGenerator Generator { get; }
    public BlackSegment(ContinuousGenerator generator, int distance) : base("black", distance)
    {
        Generator = generator;
    }

    public override double GetValue()
    {
        return Generator.Sample();
    }
}