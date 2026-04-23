namespace MainLogic.Segments;

public class BlueSegment : Segment
{
    public DiscreteGenerator Generator { get; }
    public BlueSegment(DiscreteGenerator generator, int distance) : base("blue", distance)
    {
        Generator = generator;
    }

    public override double GetValue()
    {
        return Generator.Sample();
    }
}