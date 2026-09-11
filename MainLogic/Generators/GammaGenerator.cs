namespace MainLogic;
/**
 * Kod vytvoreny s pomocou AI, zdokumentovane v kapitole 3
 */
public class GammaGenerator
{
    private readonly Random _generator;
    private readonly double _alpha; // shape parameter (k)
    private readonly double _beta;  // scale parameter (theta), mean = alpha * beta
 
    public GammaGenerator(int seed, double alpha, double beta)
    {
        if (alpha <= 0)
            throw new ArgumentException("Shape parameter alpha must be > 0");
        if (beta <= 0)
            throw new ArgumentException("Scale parameter beta must be > 0");
 
        _generator = new Random(seed);
        _alpha = alpha;
        _beta = beta;
    }
 
    // Generates a standard normal sample using Box-Muller transform
    private double NextGaussian()
    {
        double u1 = 1 - _generator.NextDouble();
        double u2 = 1 - _generator.NextDouble();
        return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
    }
 
    // Marsaglia & Tsang algorithm for alpha >= 1
    private double GenerateStandard(double alpha)
    {
        double d = alpha - 1.0 / 3.0;
        double c = 1.0 / Math.Sqrt(9.0 * d);
 
        while (true)
        {
            double x, v;
            do
            {
                x = NextGaussian();
                v = 1.0 + c * x;
            } while (v <= 0.0);
 
            v = v * v * v;
            double u = 1 - _generator.NextDouble();
 
            // Squeeze check (fast path)
            if (u < 1.0 - 0.0331 * (x * x) * (x * x))
                return d * v;
 
            // Log check (slow path, rarely reached)
            if (Math.Log(u) < 0.5 * x * x + d * (1.0 - v + Math.Log(v)))
                return d * v;
        }
    }
 
    public double Generate()
    {
        double sample;
 
        if (_alpha >= 1.0)
        {
            sample = GenerateStandard(_alpha);
        }
        else
        {
            // Boost trick for alpha < 1: Gamma(alpha) = Gamma(alpha+1) * U^(1/alpha)
            double u = 1 - _generator.NextDouble();
            sample = GenerateStandard(_alpha + 1.0) * Math.Pow(u, 1.0 / _alpha);
        }
 
        return sample * _beta; // scale to desired distribution
    }
}