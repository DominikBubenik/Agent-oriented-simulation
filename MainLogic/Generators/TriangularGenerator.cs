namespace MainLogic;
/**
 * Kod vytvoreny s pomocou AI, zdokumentovane v kapitole 1
 vzorec z https://en.wikipedia.org/wiki/Triangular_distribution Generating random variables
 */
public class TriangularGenerator
{
    private readonly Random _generator;
    private readonly double _min; 
    private readonly double _max; 
    private readonly double _mod;

    public TriangularGenerator(int seed, double min, double max, double mod)
    {
        if (!(min <= mod && mod <= max))
            throw new ArgumentException("Require min ≤ modus ≤ max");

        _generator = new Random(seed);
        _min = min;
        _max = max;
        _mod = mod;
    }

    public double Generate()
    {
        var u = _generator.NextDouble();
        var fc = (_mod - _min) / (_max - _min);

        if (u < fc)
        {
            return _min + Math.Sqrt(u * (_max - _min) * (_mod - _min));
        }
        else
        {
            return _max - Math.Sqrt((1 - u) * (_max - _min) * (_max - _mod));
        }
    }
}