namespace MainLogic.Segments;

public class RedSegment : Segment
{
    public DiscreteGenerator Generator { get; }
    public RedSegment(DiscreteGenerator generator, int distance) : base("red", distance)
    {
        Generator = generator;
    }

    public override double GetValue()
    {
        return Generator.Sample();
    }
}