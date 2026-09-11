namespace MainLogic;

public class FerryArrivalEvent(PortSimulationCore core) : PortEvents(core)
{
    // public readonly double FERRY_WAITING_TIME = 5.0;

    public override void Execute()
    {
        GlobalLogger.PrintLog("Executed FerryArrivalEvent", _core.CurrentSimulationTime, Time);
        while (GetPortCore().FerryUtilization.Count > 0)
        {
            GetPortCore().FerryUtilization.Dequeue(_core.CurrentSimulationTime);
        }
        GetPortCore().FerryInPort = true;
        while (GetPortCore().Cars.Count > 0 && GetPortCore().CanLoadFerry())
        {
            var car = GetPortCore().Cars.Dequeue(_core.CurrentSimulationTime);
            var waitTime = _core.CurrentSimulationTime - car.ArrivalTime;
            GetPortCore().WaitTimeStat.AddSample(waitTime);
            GetPortCore().FerryUtilization.Enqueue(car, _core.CurrentSimulationTime);
            var ferryDepartureEvent = new FerryDepartureEvent(GetPortCore())
            {
                Time = _core.CurrentSimulationTime //+ FERRY_WAITING_TIME
            };
            GetPortCore().FerryInPort = false;
            GlobalLogger.PrintLog("New ferryDepartureEvent", _core.CurrentSimulationTime, ferryDepartureEvent.Time);
            _core.PlanEvent(ferryDepartureEvent);
        }
        GlobalLogger.PrintLog($"Loaded count: {GetPortCore().FerryUtilization.Count}", _core.CurrentSimulationTime);
        
      
    }
}