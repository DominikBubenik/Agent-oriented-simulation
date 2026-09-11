namespace MainLogic;

public class PassengerArrivalEvent(AirportSimulationCore core)
    : AirportEvent(core, core.Lanes[0]) // core.Lanes[0] just dummy value for constructor, the real Lane is assigned in Execute method
{
    public override void Execute()
    {
        Lane = GetAirportCore().GetBestLane();
        var passenger = new Passenger(GetAirportCore().PassengersId.ToString(), _core.CurrentSimulationTime, GetAirportCore().GetLuggageCount());
        GetAirportCore().NotifyLogger($"Passenger Arrival Event {passenger.ToString()},   Time= {_core.CurrentSimulationTime}    Lane= {Lane.Id}");
        if (Lane._passengersEntryQueue.IsEmpty())
        {
            LoadLuggage(passenger);
            
            if (passenger.CurrentlyHolding == 0)
            {
                var loadTraysEvent = new AfterLoadTraysEvent(GetAirportCore(), Lane, passenger, _core.CurrentSimulationTime);
                GetAirportCore().PlanEvent(loadTraysEvent);
            }
            else
            {
                Lane.EnqueueItemToQueue(GetAirportCore().CommonEntryQueueStats, Lane._passengersEntryQueue, passenger, _core.CurrentSimulationTime);
            }
        }
        else
        {
            Lane.EnqueueItemToQueue(GetAirportCore().CommonEntryQueueStats, Lane._passengersEntryQueue, passenger, _core.CurrentSimulationTime);
        }

        GlobalLogger.PrintLog($"Passenger Arrival Event {passenger.ToString()}", _core.CurrentSimulationTime);
        GetAirportCore().NotifyRefresh();
        Time += GetAirportCore().GetNextPassangerArrival();
        GetAirportCore().PlanEvent(this);
    }
}