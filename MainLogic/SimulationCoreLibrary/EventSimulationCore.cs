namespace MainLogic;

public class EventSimulationCore : SimulationCore
{
    public event Action<double>? OnTimeUpdated;
    protected PriorityQueue<SimulationEvent, double> CalendarOfEvents { get; set; } = new ();
    public double CurrentSimulationTime { get; protected set; }  
    public double EndSimulationTime { get; protected set; }
    public bool ActivateObservationMode { get; set; }
    public double SleepEventsPeriod { get; set; } = 5.0 / 60;
    public int SleepTimeMilliseconds { get; set; } = 100;

    public readonly ManualResetEventSlim _pauseEvent = new(true);

    public override void BeforeReplication()
    {
    }

    public override void BeforeSimulation()
    {
        EndSimulation = false;
    }

    public override void AfterReplication()
    {
    }

    public override void AfterSimulation()
    {
    }
    
    public override void DoReplication()
    {
        while (CalendarOfEvents.Count > 0 && !EndSimulation) 
        {
            if (ActivateObservationMode)
            {
                PlanEvent(new SleepEvent(this){ Time = CurrentSimulationTime });
                ActivateObservationMode = false;
            }
            var simulationEvent = CalendarOfEvents.Dequeue();
            CurrentSimulationTime = simulationEvent.Time;
            if (CurrentSimulationTime >= EndSimulationTime)
            {
                break;
            }
            
            _pauseEvent.Wait();

            simulationEvent.Execute();
        }
    }

    public void PlanEvent(SimulationEvent simEvent)
    {
        if (simEvent.Time < CurrentSimulationTime)
        {
            throw new Exception("The simulation time is not correct");
        }
        CalendarOfEvents.Enqueue(simEvent, simEvent.Time);
    }
    
    public void SetPaused(bool paused)
    {
        if (paused)
            _pauseEvent.Reset();
        else
            _pauseEvent.Set();
    }
    
    public void NotifyRefresh()
    {
        OnTimeUpdated?.Invoke(CurrentSimulationTime);
    }
}