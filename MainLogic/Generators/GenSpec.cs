namespace MainLogic;

public class GenSpec(double probability, int min, int max)
{
    public double Probability { get; } = probability;
    public int Min { get; } = min;
    public int Max { get; } = max;
}