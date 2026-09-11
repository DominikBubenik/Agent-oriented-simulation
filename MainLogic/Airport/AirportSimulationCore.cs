namespace MainLogic;

/**
 * architektura systemu bola konzultovana s vyuzitim AI, zdokumentovane v kapitole 2
 */
public class AirportSimulationCore : EventSimulationCore
{
    //Handlers
    public event Action<EventSimulationCore>? OnRefreshGUI;
    public event Action<EventSimulationCore>? OnUpdateStats;
    public event Action<double>? OnUpdateGraphs;
    public event Action<string>? OnLoggerOutput;

    //Modes
    public bool TurboMode { get; set; }
    public bool SensitivityMode { get; private set; }
    public bool ObserveMode { get; set; }
    public bool WarmingProofMode { get; private set; }


    public int PassengersId { get; private set; }
    private readonly double _lambdaArrivals;
    private double _startSimulationTime;
    
    //Generators
    private readonly Random _seeder;
    private ExponentionalGenerator _passengersArrivals;
    private ContinuousGenerator _luggageDetector;
    private ContinuousGenerator _passengerDetector;
    private TriangularGenerator _personalInspection;
    private Random _personalInspectionProbability;
    private Random _luggageCoutGenerator;
    private Random _decisionOfQueue;
    
    public SimpleStat avgTimeInSystem { get; private set; }
    public int SecurityLanesCount { get; private set; }

    public List<SecurityLane> Lanes { get; } = [];

    //StatQueues
    public StatQueue<Passenger> CommonEntryQueueStats { get; private set; }
    public StatQueue<Passenger> CommonDetectorQueueStats { get; private set; }
    public StatQueue<Passenger> CommonWaitForLuggageQueueStats { get; private set; }
    public StatQueue<Luggage> CommonBeforeLugDetectorTrackQueueStats { get; private set; }
    public StatQueue<Luggage> CommonAfterLugDetectorTrackQueueStats { get; private set; }

    //Stats
    public SimpleStat TotalPassengersInSystem { get; private set; }
    public SimpleStat SumOfAvgTimeInSystem { get; private set; }
    public SimpleStat SumOfAvgEntryQueueLength { get; private set; }
    public SimpleStat SumOfAvgPassengerDetectorQueueLength { get; private set; }
    public SimpleStat SumOfAvgLuggageWaitingQueueLength { get; private set; }
    public SimpleStat SumOfAvgBeforeDetectorLug { get; private set; }
    public SimpleStat SumOfAvgAfterDetectorLug { get; private set; }

    private bool _isWarmedUp = false;
    private int _refreshRate = 0;

    public int RefreshRate
    {
        get => _refreshRate;
        set
        {
            _refreshRate = value < 0 ? 0 : value;
            RefreshCounter = 0;
        }
    }

    private int RefreshCounter = 0;

    public int SystemCapacity { get; private set; }
    public double TimeIntervalSeconds { get; private set; }
    public int TraysBeforeDetector {get; private set;}
    public int TraysAfterDetector {get; private set;}
    public int FinishWarmUp { get; set; }
    

    public AirportSimulationCore(int seed, int securityLanes, int traysBeforeDetector = 4,
        int traysAfterDetector = 5, double endSimulationTime = 600,
        bool observingMode = false, double timeIntervalSeconds = 0,
        int systemCapacity = 1000, int refreshRate = 0, bool sensitivityMode = false, bool turboMode = false, bool warmingProofMode = false, int finishedWarmUp = 20_000)
    {
        _seeder = new Random(seed);
        _lambdaArrivals = 1.0 / (timeIntervalSeconds / systemCapacity);
        _startSimulationTime = 0;
        base.EndSimulationTime = endSimulationTime + finishedWarmUp;
        CurrentSimulationTime = _startSimulationTime;
        base.ActivateObservationMode = observingMode;
        SecurityLanesCount = securityLanes;
        TimeIntervalSeconds = timeIntervalSeconds;
        SystemCapacity = systemCapacity;
        
        //Modes
        TurboMode = turboMode;
        SensitivityMode = sensitivityMode;
        ObserveMode = observingMode;
        WarmingProofMode = warmingProofMode;
        RefreshRate = refreshRate;

        FinishWarmUp = finishedWarmUp;
        TraysBeforeDetector = traysBeforeDetector;
        TraysAfterDetector = traysAfterDetector;
    }

    public override void BeforeReplication()
    {
        CurrentSimulationTime = _startSimulationTime;
        CalendarOfEvents.Clear();
        PassengersId = 0;
        avgTimeInSystem = new SimpleStat();

        //Common Queues
        CommonEntryQueueStats = new StatQueue<Passenger>(_startSimulationTime);
        CommonDetectorQueueStats = new StatQueue<Passenger>(_startSimulationTime);
        CommonWaitForLuggageQueueStats = new StatQueue<Passenger>(_startSimulationTime);
        CommonBeforeLugDetectorTrackQueueStats = new StatQueue<Luggage>(_startSimulationTime);
        CommonAfterLugDetectorTrackQueueStats = new StatQueue<Luggage>(_startSimulationTime);
        
        //Lanes
        Lanes.Clear();
        for (int i = 0; i < SecurityLanesCount; i++)
        {
            Lanes.Add(new SecurityLane(i, _startSimulationTime, TraysBeforeDetector, TraysAfterDetector));
        }

        PlanEvent(new PassengerArrivalEvent(this)
        {
            Time = CurrentSimulationTime + _passengersArrivals.Generate()
        });
        
        NotifyRefresh();
        
        PlanEvent(new WarmUpEndEvent(this)
        {
            Time = FinishWarmUp
        });
    }

    public override void AfterReplication()
    {
        if (!EndSimulation)
        {
            SumOfAvgTimeInSystem.AddSample(avgTimeInSystem.GetAverage());
            TotalPassengersInSystem.AddSample(avgTimeInSystem.GetCount());
            SumOfAvgEntryQueueLength.AddSample(CommonEntryQueueStats.GetAverageQueueLength(EndSimulationTime));
            SumOfAvgPassengerDetectorQueueLength.AddSample(CommonDetectorQueueStats.GetAverageQueueLength(EndSimulationTime));
            SumOfAvgLuggageWaitingQueueLength.AddSample(CommonWaitForLuggageQueueStats.GetAverageQueueLength(EndSimulationTime));
            SumOfAvgBeforeDetectorLug.AddSample(CommonBeforeLugDetectorTrackQueueStats.GetAverageQueueLength(EndSimulationTime));
            SumOfAvgAfterDetectorLug.AddSample(CommonAfterLugDetectorTrackQueueStats.GetAverageQueueLength(EndSimulationTime));
        }
        // OnUpdateStats?.Invoke(this);
        OnUpdateStats?.Invoke(this);
    }

    public override void BeforeSimulation()
    {
        CurrentSimulationTime = _startSimulationTime;
        _passengersArrivals = new ExponentionalGenerator(_seeder.Next(), _lambdaArrivals);
        _passengerDetector = new ContinuousGenerator(_seeder, new List<GenSpec>
        {
            new(1, 6, 27)
        });
        _luggageDetector = new ContinuousGenerator(_seeder, new List<GenSpec>
        {
            new(1, 9, 46)
        });
        _personalInspection = new TriangularGenerator(_seeder.Next(), 10, 120, 35);
        _personalInspectionProbability = new Random(_seeder.Next());
        _luggageCoutGenerator = new Random(_seeder.Next());
        _decisionOfQueue = new Random(_seeder.Next());

        //stats
        TotalPassengersInSystem = new SimpleStat();
        SumOfAvgTimeInSystem = new SimpleStat();
        SumOfAvgEntryQueueLength = new SimpleStat();
        SumOfAvgPassengerDetectorQueueLength = new SimpleStat();
        SumOfAvgLuggageWaitingQueueLength = new SimpleStat();
        SumOfAvgBeforeDetectorLug = new SimpleStat();
        SumOfAvgAfterDetectorLug = new SimpleStat();
    }

    public override void AfterSimulation()
    {
        var timeInSystem = GlobalLogger.FormatTime(SumOfAvgTimeInSystem.GetAverage());
        Console.WriteLine($"finalValue Time in system: {timeInSystem}");
        Console.WriteLine(
            $"total passengers count in system: {TotalPassengersInSystem.GetAverage()}, {TotalPassengersInSystem.GetConfidenceIntervalString()}");
        Console.WriteLine(
            $"avg entry queue length: {SumOfAvgEntryQueueLength.GetAverage()}, {SumOfAvgEntryQueueLength.GetConfidenceIntervalString()}");
        Console.WriteLine(
            $"avg passenger detector queue length: {SumOfAvgPassengerDetectorQueueLength.GetAverage()}, {SumOfAvgPassengerDetectorQueueLength.GetConfidenceIntervalString()}");
        Console.WriteLine(
            $"avg waiting luggage queue length: {SumOfAvgLuggageWaitingQueueLength.GetAverage()}, {SumOfAvgLuggageWaitingQueueLength.GetConfidenceIntervalString()}");
        OnRefreshGUI?.Invoke(this);
    }

    public double GetNextPassangerArrival()
    {
        PassengersId++;
        return _passengersArrivals.Generate();
    }

    public double GetLuggageScanDuration()
    {
        return _luggageDetector.Sample();
    }

    public bool GoToPersonalInspection()
    {
        return _personalInspectionProbability.NextDouble() < 0.19;
    }

    public double GetSecurityControlDuration()
    {
        return _passengerDetector.Sample();
    }

    public double GetPersonalInspectionDuration()
    {
        return _personalInspection.Generate();
    }

    public int GetLuggageCount()
    {
        var value = _luggageCoutGenerator.NextDouble();
        return value switch
        {
            < 0.15 => 0,
            < 0.83 => 1,
            _ => 2
        };
    }

    public void ResetStatistics()
    {
        _isWarmedUp = true;
        avgTimeInSystem.Reset();
        CommonEntryQueueStats.Reset(CurrentSimulationTime);
        CommonDetectorQueueStats.Reset(CurrentSimulationTime);
        CommonWaitForLuggageQueueStats.Reset(CurrentSimulationTime);
        CommonBeforeLugDetectorTrackQueueStats.Reset(CurrentSimulationTime);
        CommonAfterLugDetectorTrackQueueStats.Reset(CurrentSimulationTime);

        foreach (var lane in Lanes)
        {
            lane.ResetStatistics(CurrentSimulationTime);
        }
    }

    /**
     * Kod vygenerovany s pomocou AI, zdokumentovane v kapitole 2
     */
    public SecurityLane GetBestLane()
    {
        var minCount = Lanes.Min(l => l._passengersEntryQueue.Count);
        var candidates = Lanes.Where(l => l._passengersEntryQueue.Count == minCount).ToList();
        return candidates.Count == 1 ? candidates[0] : candidates[_decisionOfQueue.Next(candidates.Count)];
    }

    public void NotifyRefresh()
    {
        if (TurboMode) return;
        base.NotifyRefresh();
        var refresh = false;
        if (RefreshCounter == RefreshRate)
        {
            refresh = true;
            RefreshCounter = 0;
        }
        else
        {
            RefreshCounter++;
            return;
        }

        //Sensitivity Mode
        if (refresh && SensitivityMode)
        {
            OnUpdateGraphs?.Invoke(CurrentSimulationTime);
        }

        if (refresh && ObserveMode)
        {
            OnRefreshGUI?.Invoke(this);
        }

        if (refresh && WarmingProofMode)
        {
            OnUpdateGraphs?.Invoke(CurrentSimulationTime);
        }
    }

    public void NotifyLogger(string message)
    {
        if (!ObserveMode) return;
        OnLoggerOutput?.Invoke(message);
    }
}