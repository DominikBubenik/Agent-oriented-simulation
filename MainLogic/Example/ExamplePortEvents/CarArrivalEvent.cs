namespace MainLogic;

public class CarArrivalEvent(PortSimulationCore core) : PortEvents(core)
{
    public override void Execute()
    {
        GlobalLogger.PrintLog("Executed CarArrivalEvent", _core.CurrentSimulationTime, Time);
        
        var car = new Car(_core.CurrentSimulationTime);
        if (GetPortCore().CanLoadFerry())
        {
            GetPortCore().WaitTimeStat.AddSample(0);
            GetPortCore().FerryUtilization.Enqueue(car, _core.CurrentSimulationTime);
            var newFerryDepartureEvent = new FerryDepartureEvent(GetPortCore())
            {
                Time = _core.CurrentSimulationTime
            };
            GetPortCore().FerryInPort = false;
            base._core.PlanEvent(newFerryDepartureEvent);
        }
        else
        {
            GetPortCore().Cars.Enqueue(car, _core.CurrentSimulationTime);
        }

        Time += GetPortCore().GetArrivalToA();
        GlobalLogger.PrintLog("New carArrivalEvent", _core.CurrentSimulationTime, Time);
        base._core.PlanEvent(this);
    }
}