namespace MainLogic;

public class WarmUpEndEvent(AirportSimulationCore core) : AirportEvent(core, core.Lanes[0])
{
    public override void Execute()
    {
        GetAirportCore().ResetStatistics();
    }
}