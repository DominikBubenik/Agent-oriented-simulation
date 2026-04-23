namespace MainLogic;

public class PortSimulationCore : EventSimulationCore
{
    private ExponentionalGenerator arrivalsOnA;
    private TriangularGenerator travelTime;
    private readonly Random _seeder;
    private readonly double _startSimulationTime;
    public StatQueue<Car> Cars { get; private set; }
    public StatQueue<Car> FerryUtilization { get; private set; }
    public SimpleStat WaitTimeStat { get; private set; }
    public double TotalQueueSum = 0.0;
    public double TotalWaitingTime = 0.0;
    public double TotalFerryUtilization = 0.0;
    public bool FerryInPort { get; set; }
    public readonly int FERRY_MAX_LOAD = 1; 
    public PortSimulationCore(int seed, double startTime, double endTime)
    {
        _seeder = new  Random(seed);
        _startSimulationTime = startTime;
        base.EndSimulationTime = endTime;
    }

    public override void BeforeSimulation()
    {
        var hourIntensity = 1 / 60.0;
        arrivalsOnA = new ExponentionalGenerator(_seeder.Next(), hourIntensity);
        travelTime = new TriangularGenerator(_seeder.Next(), 5, 30, 10);

        ActivateObservationMode = true;
    }

    public override void AfterSimulation()
    {
        Console.WriteLine($"This is average queue Length {TotalQueueSum / base.CurrentReplication}");
        Console.WriteLine($"This is utilization {TotalFerryUtilization / base.CurrentReplication}");
        Console.WriteLine($"This is average waitingTime {TotalWaitingTime / base.CurrentReplication}");
    }

    public override void BeforeReplication()
    {
        Console.WriteLine("Replication" + CurrentReplication + new string('-', 50));
        
        CurrentSimulationTime = _startSimulationTime;
        Cars = new StatQueue<Car>(_startSimulationTime);
        FerryUtilization = new StatQueue<Car>(_startSimulationTime);
        WaitTimeStat = new SimpleStat();
        FerryInPort = true;
        var carArrival = new CarArrivalEvent(this); 
        carArrival.Time = _startSimulationTime;
        var departure = new FerryDepartureEvent(this); 
        departure.Time = _startSimulationTime;
        PlanEvent(carArrival);
        PlanEvent(departure);
    }

    public override void AfterReplication()
    {
        TotalQueueSum += Cars.GetAverageQueueLength(base.CurrentSimulationTime);
        TotalWaitingTime += WaitTimeStat.GetAverage();
        TotalFerryUtilization  += FerryUtilization.GetAverageQueueLength(base.CurrentSimulationTime);
    }

    public double GetArrivalToA()
    {
        // return 12.0;
        return arrivalsOnA.Generate();
    }

    public double GetTravelTime()
    {
        // return 10.0;
        return travelTime.Generate();
    }

    public bool CanLoadFerry()
    {
        return FerryInPort && FerryUtilization.Count < FERRY_MAX_LOAD;
    }
}