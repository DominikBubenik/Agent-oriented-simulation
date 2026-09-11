namespace MainLogic;
/**
 * Kod vytvoreny s pomocou AI, zdokumentovane v kapitole 1
 vzorec z //https://proofwiki.org/wiki/Probability_Generating_Function_of_Poisson_Distribution
 */
public class ExponentionalGenerator
{
    private readonly Random _generator;
    private readonly double _lambda;

    public ExponentionalGenerator(int seed, double lambda)
    {
        _generator = new Random(seed);
        _lambda = lambda;
    }
    public double Generate()
    {
        var u = 1 - _generator.NextDouble();
        return -Math.Log(u) / _lambda;
    }
}