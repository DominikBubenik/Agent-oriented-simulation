namespace MainLogic;

public abstract class AbstractGenerator <T>
{
    protected Random ProbabilityGenerator { get; set; }
    protected List<(Random generator, GenSpec spec)> CustomGenerators { get; set; }

    protected AbstractGenerator(Random randomSeeder, List<GenSpec> genSpecs)
    {
        ProbabilityGenerator = new Random(randomSeeder.Next());
        CustomGenerators = new List<(Random, GenSpec)>(genSpecs.Count);
        foreach (var genSpec in genSpecs)
        {
            CustomGenerators.Add((new Random(randomSeeder.Next()), genSpec));
        }
    }

    public T Sample()
    {
        var value = ProbabilityGenerator.NextDouble();
        var cumulation = 0.0;
        foreach (var genSpec in CustomGenerators)
        {
            cumulation += genSpec.spec.Probability;
            if (value < cumulation)
            {
                var min = genSpec.spec.Min;
                var max = genSpec.spec.Max;
                return Generate(genSpec.generator, min, max);
            }
        }
        throw new Exception("Value Not Correctly generated");
    }

    protected abstract T Generate(Random generator,int min, int max);
}