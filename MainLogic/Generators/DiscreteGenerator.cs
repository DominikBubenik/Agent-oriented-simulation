namespace MainLogic;

public class DiscreteGenerator(Random randomSeeder, List<GenSpec> genSpecs)
    : AbstractGenerator<int>(randomSeeder, genSpecs)
{
    protected override int Generate(Random generator, int min, int max)
    {
        return generator.Next(min, max);
    }
}
