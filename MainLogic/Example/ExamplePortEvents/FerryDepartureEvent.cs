namespace MainLogic;

public class FerryDepartureEvent(PortSimulationCore core) : PortEvents(core)
{
    public override void Execute()
    {
        GlobalLogger.PrintLog("Executed FerryDepartureEvent", _core.CurrentSimulationTime, Time);
        GetPortCore().FerryInPort = false;
        var ferryArrivalEvent = new FerryArrivalEvent(GetPortCore())
        {
            Time = base.Time + GetPortCore().GetTravelTime()
        };
        GlobalLogger.PrintLog("New ferryArrivalEvent", _core.CurrentSimulationTime, ferryArrivalEvent.Time);
        GetPortCore().PlanEvent(ferryArrivalEvent);
    }
}