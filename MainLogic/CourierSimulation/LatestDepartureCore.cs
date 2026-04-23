namespace MainLogic;

public class LatestDepartureCore : CourierSimulationCore
{
    // Progress callback invoked for logging / UI updates
    public Action<string>? Progress { get; set; }

    public int ArrivalsOnTime { get; set; }
    public double PercentageOfArrivalsOnTime { get; set; }
    public Random Seeder;
    private readonly int _arrivalOnTime = 455;
    public bool EndSearching { get; set; }
    public LatestDepartureCore(int seed) : base(seed)
    {
        base.StartSimulationTime = 360;
        Seeder = new Random(seed);
    }
    
    protected override void InitPaths()
    {
        Path = new string[][]
        {
            ["Z", "S", "R", "D", "Z"]//7:09:59   //["Z", "D", "R", "S", "Z"]// 7:08:39
        };
    }
    
    public override void AfterReplication()
    {
        base.AfterReplication();
        if (TotalSimulationTime <= _arrivalOnTime)
        {
            ArrivalsOnTime++;
        }
    }

    public override void BeforeSimulation()
    {
        Seed = Seeder.Next();
        base.BeforeSimulation();
        ArrivalsOnTime = 0;
    }

    public override void AfterSimulation()
    {
        base.AfterSimulation();
        PercentageOfArrivalsOnTime = ((double)ArrivalsOnTime / base.CurrentReplication) * 100;
        var msg = $"Percentage of arrivals on time: {PercentageOfArrivalsOnTime}%";
        try { Progress?.Invoke(msg); } catch { }
        Console.WriteLine(msg);
    }

    /**
     * Kod pozmeneny pomocou AI, opisane v kapitole 9
     * jedine zmeny boli pridanie Progress?.Invoke(...) volani pre zobrazovanie real-time vysledkov
     */
    public void FindLatestDeparture(double startTime = 360)
    {
        var bestTimeFound = false;
        var precisionOnSeconds = false;
        StartSimulationTime = startTime;
        var beforeWasMinus = false;
        EndSearching = false;
        
        while (!bestTimeFound && !EndSearching)
        {
            var time = TimeSpan.FromMinutes(StartSimulationTime);
            var startMsg = $"Start  at {time:hh\\:mm\\:ss} (Number value {StartSimulationTime})\n";
            try { Progress?.Invoke(startMsg); } catch { }
            Console.WriteLine(startMsg);

            RunSimulation(100_000);

            if (PercentageOfArrivalsOnTime < 80)
            {
                if (precisionOnSeconds)
                {
                    StartSimulationTime -= (1 / 60.0);
                    beforeWasMinus = true;
                }
                else
                {
                    StartSimulationTime--;
                    precisionOnSeconds = true;
                } 
            }
            else
            {
                if (precisionOnSeconds)
                {
                    StartSimulationTime += (1 / 60.0);
                    if (beforeWasMinus)
                    {
                        time = TimeSpan.FromMinutes(StartSimulationTime);
                        var bestMsg = $"This is the best time {time.Hours}:{time.Minutes}:{time.Seconds}";
                        var valMsg = $"This is vale = {StartSimulationTime}";
                        
                        try { Progress?.Invoke(" "); } catch { }
                        try { Progress?.Invoke(bestMsg); } catch { }
                        try { Progress?.Invoke(valMsg); } catch { }
                        Console.WriteLine(bestMsg);
                        Console.WriteLine(valMsg);
                        
                        bestTimeFound = true;
                    }
                }
                else
                {
                    StartSimulationTime++;
                }
            }
            
            // Also publish a summary for UI: path, arrival and value
            var pathDesc = string.Join(" ", Path[0].Select(p => p));
            var summary = $"For path:  {pathDesc}  Value: {TotalSimulationTime}";
            try { Progress?.Invoke(summary); } catch { }
            Console.WriteLine(summary);
        }
    }

    public void RunJustOnePath(string[][] path, int seed = 0, int replications = 100_000, double startTime = 360)
    {
        Path = path;
        Seeder = new Random(seed);
        StartSimulationTime = startTime;
        RunSimulation(replications);
    }
}