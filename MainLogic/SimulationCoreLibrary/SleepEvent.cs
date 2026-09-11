namespace MainLogic;

public class SleepEvent(EventSimulationCore core) : SimulationEvent(core)
{
    public override void Execute()
    {
        Thread.Sleep(core.SleepTimeMilliseconds);
        Time += core.SleepEventsPeriod;
        _core.PlanEvent(this);
        
        core.NotifyRefresh();
    }
}