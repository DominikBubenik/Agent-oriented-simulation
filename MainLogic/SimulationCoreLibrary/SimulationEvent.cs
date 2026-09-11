namespace MainLogic;

public abstract class SimulationEvent
{
    public double Time { get; set; }
    public abstract void Execute(); 
    protected EventSimulationCore _core { get; set; }
    public SimulationEvent(EventSimulationCore core)
    {
        _core = core;
    }
}