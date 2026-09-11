using ScottPlot;

namespace MainLogic;

public class GeneratorTester
{
    public DiscreteGenerator DiscreteGenerator { get; set; }
    public ContinuousGenerator ContinuousGenerator { get; set; }
    public Random SeedGenerator { get; set; }
    private List<int> _samplesDiscrete = new();
    private List<double> _samplesContinuous = new();
    
    public void TestDiscreteGenerator(int replications)
    {
        SeedGenerator = new Random(DateTime.Now.Millisecond);
        var discreteSpecifications = new List<GenSpec>
        {
            new GenSpec(0.2, 0, 20),
            new GenSpec(0.5, 20, 40),
            new GenSpec(0.05, 40, 60),
            new GenSpec(0.15, 60, 80),
            new GenSpec(0.1, 80, 100)
        };
        DiscreteGenerator = new DiscreteGenerator(SeedGenerator, discreteSpecifications);
        
        for (int i = 0; i < replications; i++)
        {
            var sample = DiscreteGenerator.Sample();
            _samplesDiscrete.Add(sample);
        }
        var doubleValues = _samplesDiscrete.ConvertAll(i => (double)i);
        GenerateVerificationHistogram(discreteSpecifications, doubleValues, "discreteHistogram ");
    }
    
    public void TestContinuousGenerator(int replications)
    {
        SeedGenerator = new Random(DateTime.Now.Millisecond);
        var continousSpecifications = new List<GenSpec>
        {
            new GenSpec(0.2, 0, 20),
            new GenSpec(0.5, 20, 40),
            new GenSpec(0.05, 40, 60),
            new GenSpec(0.15, 60, 80),
            new GenSpec(0.1, 80, 100)
        };
        ContinuousGenerator = new ContinuousGenerator(SeedGenerator, continousSpecifications);
        
        for (int i = 0; i < replications; i++)
        {
            var sample = ContinuousGenerator.Sample();
            _samplesContinuous.Add(sample);
        }
        
        GenerateVerificationHistogram(continousSpecifications,_samplesContinuous, "continuousHistogram");
    }

    public void TestExponentionalGenerator(int replications)
    {
        SeedGenerator = new Random(DateTime.Now.Microsecond);
        var expoGen = new ExponentionalGenerator(SeedGenerator.Next(), 10);
        for (int i = 0; i < replications; i++)
        {
            var sample = expoGen.Generate();
            _samplesContinuous.Add(sample);
        }

        File.WriteAllLines("exponential_samples.txt", _samplesContinuous.Select(s => s.ToString()));
    }
    
    public void TestGammaGenerator(int replications)
    {
        SeedGenerator = new Random(DateTime.Now.Microsecond);
        var gammaGen = new GammaGenerator(SeedGenerator.Next(), 7.041, 0.831);
        for (int i = 0; i < replications; i++)
        {
            var sample = gammaGen.Generate();
            _samplesContinuous.Add(sample);
        }

        File.WriteAllLines("gamma_samples.txt", _samplesContinuous.Select(s => s.ToString()));
    }
    
    public void TestTriangularGenerator(int replications)
    {
        SeedGenerator = new Random(DateTime.Now.Microsecond);
        var expoGen = new TriangularGenerator(SeedGenerator.Next(), 10, 70,20);
        for (int i = 0; i < replications; i++)
        {
            var sample = expoGen.Generate();
            _samplesContinuous.Add(sample);
        }

        File.WriteAllLines("triangular_samples.txt", _samplesContinuous.Select(s => s.ToString()));
    }

    /**
     * Kod vytvoreny s pomocou AI, zdokuemtovane v kapitole 1
     */
    private void GenerateVerificationHistogram(List<GenSpec> specs, List<double> samples, string name)
    {
        //testing if there is good portion of values
        // samples = samples.Where(s => s >= 10).ToList();
        var plt = new ScottPlot.Plot();
        int totalSamples = samples.Count;

        // 1. Prepare data for the bars
        List<Bar> bars = new();
    
        for (int i = 0; i < specs.Count; i++)
        {
            var spec = specs[i];
        
            // Calculate empirical probability (Actual)
            double actualCount = samples.Count(s => s >= spec.Min && s < spec.Max);
            double actualProb = actualCount / totalSamples;

            // Create a bar for each specification bin
            bars.Add(new Bar()
            {
                Value = actualProb,
                Position = i,
                FillColor = Colors.SteelBlue,
                Label = $"Actual",
                Size = 0.4 // Width of the bar
            });

            // Optional: Add a second bar or a marker to show the "Target" probability
            bars.Add(new Bar()
            {
                Value = spec.Probability,
                Position = i + 0.4, // Offset slightly to the right
                FillColor = Colors.Orange.WithAlpha(0.6),
                Label = "Expected",
                Size = 0.4
            });
        }

        // 2. Add bars to the plot
        var barPlot = plt.Add.Bars(bars);

        // 3. Formatting the plot
        plt.Title($"Verification: {name}");
        plt.YLabel("Probability");
        plt.XLabel("Ranges (GenSpec)");

        // Setup X-Axis ticks to show the ranges
        Tick[] ticks = specs.Select((s, idx) => 
            new Tick(idx + 0.2, $"[{s.Min}-{s.Max})")).ToArray();
        plt.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);

        // Set Y axis limits (0 to slightly above 1.0 or max probability)
        plt.Axes.SetLimitsY(0, specs.Max(s => s.Probability) * 1.2);

        // 4. Save the result
        plt.SavePng($"{name}.png", 800, 600);
    }
}