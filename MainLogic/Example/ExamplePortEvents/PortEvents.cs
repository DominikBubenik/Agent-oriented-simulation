namespace MainLogic;

public abstract class PortEvents(PortSimulationCore core) : SimulationEvent(core)
{
    protected PortSimulationCore GetPortCore() => (PortSimulationCore)_core;
}