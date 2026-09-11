namespace MainLogic;

public class AfterLoadTraysEvent : AirportEvent
{
    private Passenger _passenger;

    public AfterLoadTraysEvent(AirportSimulationCore core, SecurityLane lane, Passenger passenger, double time) : base(core, lane)
    {
        _passenger = passenger;
        Time = time;
    }

    public override void Execute()
    {
        GetAirportCore().NotifyLogger($"Finished AfterLuggageLoad Event {_passenger.ToString()},   Time= {_core.CurrentSimulationTime}");
        if (_passenger.CurrentlyHolding > 0) throw new Exception("Passenger should be empty");
        
        if (Lane.DetectorQueue.IsEmpty() && !Lane.IsSecurityGuardBusy)
        {
            Lane.PassengerInsideDetector.Add(_passenger);
            Lane.IsSecurityGuardBusy = true;
            var nextSecurity = new SecurityControlEvent(GetAirportCore(), _passenger, Lane)
            {
                Time = _core.CurrentSimulationTime + GetAirportCore().GetSecurityControlDuration() //this was added  GetAirportCore().GetSecurityControlDuration() 
            };
            _core.PlanEvent(nextSecurity);
        }
        else
        {
            // Lane.DetectorQueue.Enqueue(_passenger, _core.CurrentSimulationTime);
            Lane.EnqueueItemToQueue(GetAirportCore().CommonDetectorQueueStats, Lane.DetectorQueue, _passenger, _core.CurrentSimulationTime);
        }
        
        if (!Lane.IsLuggageScannerBusy && !Lane._beforeDetectorTrack.IsEmpty() && Lane._afterDetectorTrack.Count < Lane.MAX_AFTER_DETECTOR_COUNT)
        {
            Lane.IsLuggageScannerBusy = true;
            // var luggage =  Lane._beforeDetectorTrack.Dequeue(_core.CurrentSimulationTime);
            var luggage = Lane.DequeueItemFromQueue(GetAirportCore().CommonBeforeLugDetectorTrackQueueStats, Lane._beforeDetectorTrack, _core.CurrentSimulationTime);
            Lane.LuggageInsideDetector.Add(luggage);
            var finishScan = new FinishedScanningEvent(GetAirportCore(), luggage, Lane)
            {
                Time = _core.CurrentSimulationTime + GetAirportCore().GetLuggageScanDuration()
            };

            _core.PlanEvent(finishScan);
        }
        
        if (!Lane._passengersEntryQueue.IsEmpty())
        {
            var passenger = Lane._passengersEntryQueue.Peek();
            LoadLuggage(passenger, true);
        }
        GlobalLogger.PrintLog($"Loading of passenger {_passenger.ToString()}",  _core.CurrentSimulationTime);
        GetAirportCore().NotifyRefresh();
    }
}