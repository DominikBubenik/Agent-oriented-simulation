namespace MainLogic;

public class ContinuousGenerator(Random probabilityGenerator, List<GenSpec> genSpecs)
    : AbstractGenerator<double>(probabilityGenerator, genSpecs)
{
    protected override double Generate(Random generator, int min, int max)
    {
        return generator.NextDouble() * (max - min) + min;
    }
}