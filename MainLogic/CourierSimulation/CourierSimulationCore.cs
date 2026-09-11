using MainLogic.Segments;

namespace MainLogic;

public class CourierSimulationCore : SimulationCore
{
    private DiscreteGenerator BlueRoadGenerator { get; set; }
    private ContinuousGenerator BlackRoadGenerator { get; set; }
    private DiscreteGenerator RedRoadGenerator { get; set; }
    private ContinuousGenerator GreenRoadGenerator { get; set; }
    private Random SeedGenerator { get; set; }
    private Random KSlowdownGenerator { get; set; }
    public string[][] Path { get; set; }
    private List<Segment[]> FromZilinaToDevinka { get; set; }
    private List<Segment[]> FromStrecnoToRajec { get; set; }
    private List<Segment[]> FromDivinkaToRajec { get; set; }
    private List<Segment[]> FromZilinaToStrecno { get; set; }
    private List<Segment> ZilinaKDivinka { get; set; }
    private List<Segment> ZilinaKRajecke { get; set; }
    private List<Segment> ZilinaKStrecno { get; set; }
    private List<Segment> DivinkaKStrecno { get; set; }
    private List<Segment> DivinkaKRajecke { get; set; }
    private List<Segment> RajeckeKStrecno { get; set; }
    private double[] TotalTime { get; set; } 
    protected double TotalSimulationTime { get; set; } 
    public double[] AvgTimes { get; set; }
    public double[] TotalSums { get; set; }
    public double StartSimulationTime { get; set; }
    public bool InitDone { get; set; } = false;
    protected int Seed { get; set; }
    
    
    public CourierSimulationCore(int seed)
    {
        Seed = seed;
        StartSimulationTime = 360;
    }

    private void InitGenerators()
    {
        SeedGenerator = new Random(Seed);
        var continuousSpecifications = new List<GenSpec>
        {
            new(0.1, 10, 20),
            new(0.5, 20, 32),
            new(0.2, 32, 45),
            new(0.15, 45, 75),
            new(0.05, 75, 85)
        };
        
        var discreteSpecifications = new List<GenSpec>
        {
            new(0.2, 15, 29),
            new(0.4, 29, 45),
            new(0.4, 45, 65)
        };
        BlueRoadGenerator = new DiscreteGenerator(SeedGenerator, discreteSpecifications);
        BlackRoadGenerator = new ContinuousGenerator(SeedGenerator, continuousSpecifications);
        
        var redSpecs = new List<GenSpec>();
        redSpecs.Add(new GenSpec(1, 55, 76));
        RedRoadGenerator = new DiscreteGenerator(SeedGenerator, redSpecs);
        var greenSpecs = new List<GenSpec>();
        greenSpecs.Add(new GenSpec(1, 50, 80));
        GreenRoadGenerator = new ContinuousGenerator(SeedGenerator, greenSpecs);

        KSlowdownGenerator = new Random(SeedGenerator.Next());
    }

    protected virtual void InitPaths()
    {
        Path = new string[][]
        {
            ["Z", "D", "R", "S", "Z"],
            ["Z", "D", "S", "R", "Z"],
            ["Z", "R", "D", "S", "Z"],
            ["Z", "R", "S", "D", "Z"],
            ["Z", "S", "R", "D", "Z"],
            ["Z", "S", "D", "R", "Z"]
        };
    }

    public override void DoReplication()
    {
        for (int i = 0; i < Path.Length; i++)
        {
            var current = Path[i][0];
            TotalTime[i] = StartSimulationTime;
            TotalSimulationTime = TotalTime[i];
            for (int j = 0; j < Path[i].Length - 1; j++)
            {
                var next = Path[i][j + 1];
                var direct = TravelToPlace(current, next);
                var viaK = GetTravelViaK(current, next);
                TotalTime[i] += direct < viaK ? direct : viaK;
                TotalSimulationTime = TotalTime[i];
                current = next;
            }   
        }
    }

    private double TravelToPlace(string from, string to)
    {
        var length = 0.0;
        if ((from == "Z" && to == "D") || (from == "D" && to == "Z"))
        {
            var first = FromZilinaToDevinka[0][0].GetTimeLength();
            var second = FromZilinaToDevinka[1][0].GetTimeLength();
            length += first < second ? first : second;
            return length;
        } else if ((from == "R" && to == "Z") || (from == "R" && to == "Z"))
        {
            //return GetTravelViaK(from, to);
        } else if ((from == "S" && to == "D") || (from == "D" && to == "S"))
        {
            //return  GetTravelViaK(from, to);
        } else if ((from == "S" && to == "R") || (from == "R" && to == "S"))
        {
            foreach (var road in FromStrecnoToRajec)
            {
                if (road.Length > 1)
                {
                    var first = road[0].GetTimeLength();
                    var second = road[1].GetTimeLength();
                    length += first < second ? first : second;
                }
                else
                {
                    length += road[0].GetTimeLength();
                }
            }
            return length;
        } else if ((from == "R" && to == "D") || (from == "D" && to == "R"))
        {
            for (var i = 0; i < FromDivinkaToRajec.Count; i++)
            {
                if (FromDivinkaToRajec[i].Length > 1)
                {
                    var first = FromDivinkaToRajec[i][0].GetTimeLength();
                    var second = FromDivinkaToRajec[i][1].GetTimeLength();
                    var third = FromDivinkaToRajec[i][2].GetTimeLength();
                    length += first < second + third ? first : second + third;
                }
                else
                {
                    length += FromDivinkaToRajec[i][0].GetTimeLength();
                }
            }
            return length;
        } else if ((from == "S" && to == "Z") || (from == "Z" && to == "S"))
        {
            length = double.MaxValue;
            foreach (var road in FromZilinaToStrecno)
            {
                var current = road.Sum(segment => segment.GetTimeLength());
                if (current < length)
                {
                    length = current;   
                }
            }
            return length;
        }
        return double.MaxValue;
    }
    
    private double GetTravelViaK(string from, string to)
    {
        return from switch
        {
            "Z" when to == "D" => CalculateKLenght(ZilinaKDivinka),
            "D" when to == "Z" => CalculateKLenght(Enumerable.Reverse(ZilinaKDivinka).ToList()),
            "Z" when to == "R" => CalculateKLenght(ZilinaKRajecke),
            "R" when to == "Z" => CalculateKLenght(Enumerable.Reverse(ZilinaKRajecke).ToList()),
            "Z" when to == "S" => CalculateKLenght(ZilinaKStrecno),
            "S" when to == "Z" => CalculateKLenght(Enumerable.Reverse(ZilinaKStrecno).ToList()),
            "D" when to == "R" => CalculateKLenght(DivinkaKRajecke),
            "R" when to == "D" => CalculateKLenght(Enumerable.Reverse(DivinkaKRajecke).ToList()),
            "D" when to == "S" => CalculateKLenght(DivinkaKStrecno),
            "S" when to == "D" => CalculateKLenght(Enumerable.Reverse(DivinkaKStrecno).ToList()),
            "R" when to == "S" => CalculateKLenght(RajeckeKStrecno),
            "S" when to == "R" => CalculateKLenght(Enumerable.Reverse(RajeckeKStrecno).ToList()),
            _ => double.MaxValue
        };
    }

    private double CalculateKLenght(List<Segment> list)
    {
        var time = list[0].GetTimeLength();
        if (time + TotalSimulationTime >= 390)
        {
            time += (list[1].Distance / (list[1].GetValue() * ((100 - GetKSLowDown()) / 100))) * 60;
        }
        else
        {
            time += list[1].GetTimeLength();
        }
        return time;
    }
    
    private double GetKSLowDown()
    {
        return KSlowdownGenerator.NextDouble()  * (25 - 10) + 10;
    }
    
    public override void BeforeReplication() {}

    public override void BeforeSimulation()
    {
        InitGenerators();
        InitPaths();
        
        TotalSums =  new double[Path.Length];
        TotalTime = new double[Path.Length];
        AvgTimes = new double[Path.Length];
        
        FromZilinaToDevinka = new List<Segment[]>();
        FromZilinaToDevinka.Add([new GreenSegment(GreenRoadGenerator,4)]);
        FromZilinaToDevinka.Add([new RedSegment(RedRoadGenerator, 4)]);

        FromStrecnoToRajec = new List<Segment[]>();
        FromStrecnoToRajec.Add([new BlueSegment(BlueRoadGenerator,5),  new BlackSegment(BlackRoadGenerator, 5)]);
        FromStrecnoToRajec.Add([new BlueSegment(BlueRoadGenerator,8)]);

        FromDivinkaToRajec = new List<Segment[]>();
        FromDivinkaToRajec.Add([new BlackSegment(BlackRoadGenerator,1)]);
        FromDivinkaToRajec.Add([
            new RedSegment(RedRoadGenerator,3), 
            new RedSegment(RedRoadGenerator,2),
            new BlueSegment(BlueRoadGenerator,1)
        ]);
        FromDivinkaToRajec.Add([new BlueSegment(BlueRoadGenerator,1)]);

        FromZilinaToStrecno = new List<Segment[]>();
        FromZilinaToStrecno.Add([new RedSegment(RedRoadGenerator,3), new RedSegment(RedRoadGenerator, 4)]);
        FromZilinaToStrecno.Add([new GreenSegment(GreenRoadGenerator,4), new BlackSegment(BlackRoadGenerator, 3)]);

        ZilinaKDivinka = new List<Segment>();
        ZilinaKDivinka.Add(new BlackSegment(BlackRoadGenerator,2));
        ZilinaKDivinka.Add(new RedSegment(RedRoadGenerator,2));

        ZilinaKRajecke = new List<Segment>();
        ZilinaKRajecke.Add(new BlackSegment(BlackRoadGenerator,2));
        ZilinaKRajecke.Add(new GreenSegment(GreenRoadGenerator,2));

        ZilinaKStrecno = new List<Segment>();
        ZilinaKStrecno.Add(new BlackSegment(BlackRoadGenerator,2));
        ZilinaKStrecno.Add(new BlueSegment(BlueRoadGenerator,4));
        
        DivinkaKRajecke = new List<Segment>();
        DivinkaKRajecke.Add(new RedSegment(RedRoadGenerator,2));
        DivinkaKRajecke.Add(new GreenSegment(GreenRoadGenerator,2));

        DivinkaKStrecno = new List<Segment>();
        DivinkaKStrecno.Add(new RedSegment(RedRoadGenerator,2));
        DivinkaKStrecno.Add(new BlueSegment(BlueRoadGenerator,4));
        
        RajeckeKStrecno = new List<Segment>();
        RajeckeKStrecno.Add(new GreenSegment(GreenRoadGenerator!,2));
        RajeckeKStrecno.Add(new BlueSegment(BlueRoadGenerator,4));

        EndSimulation = false;
        InitDone = true;
    }
    
    public override void AfterReplication()
    {
        for (var i = 0; i < TotalSums.Length; i++)
        {
            TotalSums[i] += TotalTime[i];
        }
    }
    
    public override void AfterSimulation()
    {
        for (var i = 0; i < Path.Length; i++)
        {
            var value = TotalSums[i] / CurrentReplication;
            var time = TimeSpan.FromMinutes(value);
            AvgTimes[i] = value;
            Console.Write("For path: ");
            foreach (var road in Path[i])
            {
                Console.Write($" {road} ");
            }
            Console.WriteLine();
            Console.WriteLine("Arrival Time: " + time.Hours + ":" + time.Minutes + ":" + time.Seconds);
            Console.WriteLine("Value " + value + "\n");   
        }
    }
}